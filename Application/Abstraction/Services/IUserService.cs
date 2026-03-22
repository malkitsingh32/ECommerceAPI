using Application.Handler.User.Dtos;
using Common.Common;

namespace Application.Abstraction.Services
{
    public interface IUserService
    {
        Task<CommonResultResponseDto<int>> AddUser(UserDto users, string password);
        Task<CommonResultResponseDto<UserDto>> GetUserByUserId(int userId);
        Task<CommonResultResponseDto<UserDto>> Login(LoginDto loginDto);
    }
}
