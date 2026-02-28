using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Options;
using Application.Abstraction.Repositories;
using Helper.Settings;
using Application.Handler.User.Dtos;
using Domain.Entities;
using Infrastructure.Implementation.Services;
using Common.Common;
using Microsoft.Extensions.Caching.Distributed;

namespace Infrastructure.Benchmarks
{
    [MemoryDiagnoser]
    public class UserServiceBenchmarks
    {
        private UserService _serviceWithInMemoryCache;
        private UserService _serviceWithDistributedCache;
        private FakeUserRepository _repo;
        private FakeDistributedCache _fakeDistributedCache;

        [GlobalSetup]
        public void Setup()
        {
            _repo = new FakeUserRepository();
            var appSettings = Options.Create(new AppSettings { Secret = "ThisIsASecretForBenchmarks1234567890" });
            var passwordHasher = new Infrastructure.Implementation.Services.PasswordHasher();
            
            // Setup in-memory cache version
            var inMemoryTokenCache = new Infrastructure.Implementation.Services.InMemoryTokenCache();
            _serviceWithInMemoryCache = new UserService(_repo, appSettings, passwordHasher, inMemoryTokenCache);
            
            // Setup distributed cache version (with fake Redis-like cache)
            _fakeDistributedCache = new FakeDistributedCache();
            var distributedTokenCache = new DistributedTokenCache(_fakeDistributedCache);
            _serviceWithDistributedCache = new UserService(_repo, appSettings, passwordHasher, distributedTokenCache);
        }

        [Benchmark]
        public async Task LoginBenchmark()
        {
            var login = new LoginDto
            {
                Email = "bench@test.local",
                Password = "P@ssw0rd!"
            };
            await _serviceWithInMemoryCache.Login(login);
        }

        [Benchmark]
        public Task JwtCreationBenchmark()
        {
            var testUser = new Users
            {
                UserId = 1,
                Email = "bench@test.local",
                UserName = "benchmark_user"
            };
            return _serviceWithInMemoryCache.CreateJWTToken(testUser);
        }

        [Benchmark]
        public async Task RepeatedLoginBenchmark_InMemory()
        {
            var login = new LoginDto
            {
                Email = "bench@test.local",
                Password = "P@ssw0rd!"
            };
            for (int i = 0; i < 5; i++)
            {
                await _serviceWithInMemoryCache.Login(login);
            }
        }

        [Benchmark]
        public async Task RepeatedLoginBenchmark_Distributed()
        {
            _fakeDistributedCache.Clear();
            var login = new LoginDto
            {
                Email = "bench@test.local",
                Password = "P@ssw0rd!"
            };
            for (int i = 0; i < 5; i++)
            {
                await _serviceWithDistributedCache.Login(login);
            }
        }

        private class FakeUserRepository : IUserRepository
        {
            private Users _user;
            public FakeUserRepository()
            {
                var password = "P@ssw0rd!";
                using (var hmac = new System.Security.Cryptography.HMACSHA512())
                {
                    var salt = hmac.Key;
                    var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                    _user = new Users
                    {
                        UserId = 1,
                        Email = "bench@test.local",
                        PasswordHash = hash,
                        PasswordSalt = salt
                    };
                }
            }

            public Task<int> InsertUser(Users users, byte[] passwordHash, byte[] passwordSalt)
            {
                users.UserId = 2;
                return Task.FromResult(users.UserId.Value);
            }

            public Task<Users> GetUserByEmail(string email)
            {
                return Task.FromResult(_user);
            }

            public Task<Users> GetUserByUserId(int userId)
            {
                return Task.FromResult(_user);
            }

            public Task<bool> DeleteUser(int userId) => throw new NotImplementedException();
            public Task<bool> UpdateUser(Users users) => throw new NotImplementedException();
        }

        // Fake distributed cache for benchmarking without actual Redis
        private class FakeDistributedCache : IDistributedCache
        {
            private readonly Dictionary<string, (byte[] data, DateTimeOffset? absoluteExpiration)> _store = new();

            public byte[] Get(string key)
            {
                if (_store.TryGetValue(key, out var entry))
                {
                    if (entry.absoluteExpiration == null || entry.absoluteExpiration > DateTimeOffset.UtcNow)
                    {
                        return entry.data;
                    }
                    _store.Remove(key);
                }
                return null;
            }

            public async Task<byte[]> GetAsync(string key, System.Threading.CancellationToken token = default)
            {
                return Get(key);
            }

            public void Set(string key, byte[] value, DistributedCacheEntryOptions options)
            {
                var absoluteExpiration = options?.AbsoluteExpiration;
                _store[key] = (value, absoluteExpiration);
            }

            public async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, System.Threading.CancellationToken token = default)
            {
                Set(key, value, options);
            }

            public void Remove(string key)
            {
                _store.Remove(key);
            }

            public async Task RemoveAsync(string key, System.Threading.CancellationToken token = default)
            {
                Remove(key);
            }

            public void Refresh(string key)
            {
                // Not needed for this benchmark
            }

            public async Task RefreshAsync(string key, System.Threading.CancellationToken token = default)
            {
                // Not needed for this benchmark
            }

            public void Clear()
            {
                _store.Clear();
            }
        }
    }
}