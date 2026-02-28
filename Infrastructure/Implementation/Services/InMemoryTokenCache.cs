using System;
using System.Collections.Concurrent;
using Application.Abstraction.Services;
using Application.Handler.User.Dtos;

namespace Infrastructure.Implementation.Services
{
    public class InMemoryTokenCache : ITokenCache
    {
        private readonly ConcurrentDictionary<int, CachedToken> _cache = new();

        private class CachedToken
        {
            public UserDto UserDto { get; set; }
            public DateTime ExpiresAt { get; set; }
        }

        public UserDto GetCachedToken(int userId, DateTime expirationThreshold)
        {
            if (_cache.TryGetValue(userId, out var cachedToken))
            {
                // Check if token is still valid (not expired and expires after the threshold)
                if (cachedToken.ExpiresAt > expirationThreshold)
                {
                    return cachedToken.UserDto;
                }
                else
                {
                    // Remove expired token
                    _cache.TryRemove(userId, out _);
                }
            }
            return null;
        }

        public void CacheToken(int userId, UserDto userDto)
        {
            var cachedToken = new CachedToken
            {
                UserDto = userDto,
                ExpiresAt = userDto.TokenExpire
            };
            _cache.AddOrUpdate(userId, cachedToken, (key, old) => cachedToken);
        }

        public void InvalidateToken(int userId)
        {
            _cache.TryRemove(userId, out _);
        }
    }
}
