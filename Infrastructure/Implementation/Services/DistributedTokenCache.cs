using System;
using System.Text.Json;
using System.Threading.Tasks;
using Application.Abstraction.Services;
using Application.Handler.User.Dtos;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Implementation.Services
{
    public class DistributedTokenCache : ITokenCache
    {
        private readonly IDistributedCache _cache;

        public DistributedTokenCache(IDistributedCache cache)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public UserDto GetCachedToken(int userId, DateTime expirationThreshold)
        {
            var key = GetCacheKey(userId);
            var cachedData = _cache.GetString(key);

            if (string.IsNullOrEmpty(cachedData))
                return null;

            try
            {
                var cachedToken = JsonSerializer.Deserialize<CachedTokenData>(cachedData);
                if (cachedToken?.ExpiresAt > expirationThreshold)
                {
                    // Return minimal UserDto with only cached token data
                    return new UserDto
                    {
                        Token = cachedToken.Token,
                        TokenExpire = cachedToken.ExpiresAt
                    };
                }
                else
                {
                    // Remove expired token
                    _cache.Remove(key);
                }
            }
            catch
            {
                // Deserialization failed, remove invalid cache entry
                _cache.Remove(key);
            }

            return null;
        }

        public void CacheToken(int userId, UserDto userDto)
        {
            var key = GetCacheKey(userId);
            // Cache only the token string and expiration, not the entire UserDto
            var cachedToken = new CachedTokenData
            {
                Token = userDto.Token,
                ExpiresAt = userDto.TokenExpire
            };

            var serialized = JsonSerializer.Serialize(cachedToken);
            var cacheOptions = new DistributedCacheEntryOptions
            {
                AbsoluteExpiration = userDto.TokenExpire
            };

            _cache.SetString(key, serialized, cacheOptions);
        }

        public void InvalidateToken(int userId)
        {
            var key = GetCacheKey(userId);
            _cache.Remove(key);
        }

        private static string GetCacheKey(int userId) => $"token:{userId}";

        private class CachedTokenData
        {
            public string Token { get; set; }
            public DateTime ExpiresAt { get; set; }
        }
    }
}
