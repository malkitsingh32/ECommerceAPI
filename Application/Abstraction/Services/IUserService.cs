using Application.Handler.User.Dtos;
using Common.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Abstraction.Services
{
    public interface IUserService
    {
        Task<CommonResultResponseDto<int>> AddUser(Users users, string password);
        Task<CommonResultResponseDto<UserDto>> GetUserByUserId(int userId);
        Task<CommonResultResponseDto<UserDto>> Login(LoginDto loginDto);
    }
}
