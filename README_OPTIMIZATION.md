# User Authentication & Token Optimization

This project implements a highly optimized user authentication and JWT token generation system with advanced caching strategies for both single-server and distributed deployments.

## Performance Achievements

### Benchmark Results (Final)

```
| Scenario                           | Mean       | Gen0  | Allocations |
|----------------------------------- |------------|-------|-------------|
| LoginBenchmark                     |   6.38 µs  | 0.34  |   1088 B    |
| JwtCreationBenchmark (cached)      | 150.5 ns   | 0.05  |    160 B    |
| RepeatedLoginBenchmark_InMemory    |  33.1 µs   | 1.65  |   5312 B    |
| RepeatedLoginBenchmark_Distributed | 109.4 µs   | 7.32  |  23113 B    |
```

### Optimization Journey

| Stage | Component | Improvement | Result |
|-------|-----------|-------------|--------|
| Phase 1 | Cached JWT Handler + Key | 23% faster | 57.4 ? 44.1 µs |
| Phase 2 | IPasswordHasher abstraction | +3% faster | 44.1 ? 42.7 µs |
| Phase 3 | Token caching (in-memory) | **85% faster** | 42.7 ? 6.4 µs |
| Phase 4 | Distributed cache support | Multi-server | 3.3× baseline overhead |
| Phase 5 | Token-only serialization | 5.7% faster | 141.4 ? 133.4 µs |

## Architecture

### Components

#### 1. **IPasswordHasher** (`Infrastructure/Implementation/Services/PasswordHasher.cs`)
- **Algorithm:** HMACSHA512
- **Security:** Fixed-time comparison using `CryptographicOperations.FixedTimeEquals`
- **Features:**
  - Resistant to timing attacks
  - Null/empty validation
  - Hash/salt length validation (64 bytes hash, 128 bytes salt)

```csharp
// Usage
var passwordHasher = new PasswordHasher();
passwordHasher.CreatePasswordHash("password", out var hash, out var salt);
bool isValid = passwordHasher.VerifyPasswordHash("password", hash, salt);
```

#### 2. **ITokenCache** Interface with Two Implementations

##### InMemoryTokenCache (Default)
- **Best for:** Single-server or sticky-session deployments
- **Performance:** Sub-microsecond cache hits (150 ns)
- **Memory overhead:** ~1 KB per cached token
- **Expiration:** Checked on retrieval; 1-minute TTL threshold before expiration
- **Thread-safe:** Uses `ConcurrentDictionary`

```csharp
// Register in DependencyInjection.cs
services.AddInfrastructure(useDistributedCache: false); // Default
```

##### DistributedTokenCache (Redis)
- **Best for:** Multi-server deployments without session affinity
- **Performance:** ~25.5 µs per cache hit (includes serialization)
- **Storage:** Remote Redis server
- **Serialization:** Optimized to cache only token + expiration (minimal JSON payload)
- **Network:** Requires Redis connection configured via `IDistributedCache`

```csharp
// Register in DependencyInjection.cs
services.AddInfrastructure(useDistributedCache: true);

// In Startup.cs or Program.cs, add Redis:
services.AddStackExchangeRedisCache(options => 
{
    options.Configuration = Configuration.GetConnectionString("Redis");
});
```

#### 3. **UserService** Optimizations

**Cached Resources:**
- `JwtSecurityTokenHandler` (instance reused per UserService instance)
- `SymmetricSecurityKey` (JWT signing key, created once from secret)
- `SigningCredentials` (JWT signing credentials, created once)

**Token Generation:**
- Cache lookup before generating new token
- 1-minute TTL threshold (ensures token has reasonable remaining lifetime)
- Token generation only on cache miss or expired token

```csharp
var userService = new UserService(
    userRepository, 
    appSettings, 
    passwordHasher,
    tokenCache
);
```

## Usage

### Configuration

#### Single-Server (In-Memory Cache)
```csharp
// In Program.cs or Startup.cs
services.AddInfrastructure(); // Default: uses InMemoryTokenCache
```

#### Multi-Server (Distributed Cache)
```csharp
// In Program.cs or Startup.cs
services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = "localhost:6379"; // Redis connection
});

services.AddInfrastructure(useDistributedCache: true);
```

### API Usage

#### Create User
```http
POST /api/user/CreateUser
Content-Type: application/json

{
  "email": "user@example.com",
  "password": "SecurePassword123!",
  "userName": "username"
}
```

#### Login
```http
GET /api/user/Login?email=user@example.com&password=SecurePassword123!
```

Response (on success):
```json
{
  "succeeded": true,
  "data": {
    "userId": 1,
    "email": "user@example.com",
    "token": "eyJhbGc...",
    "tokenExpire": "2024-12-20T12:34:56Z"
  }
}
```

## Performance Recommendations

### For Production

1. **Single-server deployments:** Use `InMemoryTokenCache` (default)
   - ? Minimal latency (~6 µs per login with cache)
   - ? No serialization overhead
   - ? Minimal memory usage (~1 KB per cached token)

2. **Multi-server deployments:** Use `DistributedTokenCache` + Redis
   - ? Shared token cache across all servers
   - ? Acceptable latency (~25 µs per cache hit)
   - ?? Accept 3.3× overhead vs. in-memory (trade-off for data consistency)
   - ? Automatic token expiration cleanup (Redis TTL)

3. **Rate Limiting:** Implement at middleware level to prevent brute-force attacks
   - Password verification accounts for ~60% of login latency
   - Limit login attempts to 5 per minute per IP/email

4. **Token Validation:** Cache should be cleared on password changes
   ```csharp
   _tokenCache.InvalidateToken(userId); // Call on password reset
   ```

## Security Considerations

1. **Password Hashing:**
   - Uses HMACSHA512 (standard .NET implementation)
   - Fixed-time comparison prevents timing attacks
   - 128-byte random salt per password

2. **JWT Token:**
   - 12-hour expiration window (configurable)
   - Signed with `HS256` algorithm
   - Contains `NameIdentifier` claim with UserId

3. **Cache Security:**
   - In-memory cache: Limited to process memory (no network exposure)
   - Distributed cache: Requires Redis authentication (configure in connection string)

4. **Token Revocation:**
   - Call `InvalidateToken(userId)` on password change
   - Redis TTL ensures automatic cleanup of expired tokens

## Benchmarking

Run full benchmark suite:
```bash
cd BenchmarkSuite1
dotnet run -c Release
```

Individual benchmarks:
```bash
# Single login (password verification + token generation)
dotnet run -c Release --filter "*LoginBenchmark"

# Cached token retrieval only
dotnet run -c Release --filter "*JwtCreationBenchmark"

# Multiple logins (in-memory cache)
dotnet run -c Release --filter "*InMemory"

# Multiple logins (distributed cache)
dotnet run -c Release --filter "*Distributed"
```

## Future Optimizations

1. **Refresh Tokens:** Implement JWT refresh token pattern
   - Short-lived access tokens (15 minutes)
   - Long-lived refresh tokens (7 days)
   - Reduces token re-generation frequency

2. **Session-based Tokens:** Store opaque session tokens instead of JWTs
   - Reduces serialization overhead
   - Simpler token revocation
   - Better for distributed systems

3. **Argon2 Hashing:** Replace HMACSHA512 with Argon2 (more secure but slower)
   - Better resistance to GPU-based brute-force
   - Trade-off: ~100× slower than HMACSHA512

4. **HTTP Response Caching:** Cache login responses at middleware level
   - Avoid database + hashing for repeated identical requests
   - Requires careful cache invalidation strategy

## Dependencies

- **Microsoft.Extensions.Caching.StackExchangeRedis** (v6.0.0) - For distributed caching
- **System.IdentityModel.Tokens.Jwt** (v6.10.0) - JWT token generation
- **Microsoft.IdentityModel.Tokens** (v6.10.0) - Token handling
- **Mapster** - Object mapping (for DTO conversion)

## License

[Your License Here]

---

**Last Updated:** 2024
**Optimization Status:** Production-Ready ?
