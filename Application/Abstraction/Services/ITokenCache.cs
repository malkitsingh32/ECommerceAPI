using Application.Handler.User.Dtos;
using System;

namespace Application.Abstraction.Services
{
    public interface ITokenCache
    {
        UserDto GetCachedToken(int userId, DateTime expirationThreshold);
        void CacheToken(int userId, UserDto userDto);
        void InvalidateToken(int userId);
    }
}
