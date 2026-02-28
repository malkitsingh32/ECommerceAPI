using Application.Handler.User.Dtos;
using Domain.Entities;

namespace Application.Abstraction.Repositories
{
    public interface IUserRepository
    {
        Task<int> InsertUser(Users user, byte[] passwordHash, byte[] passwordSalt);
        Task<Users> GetUserByUserId(int userId);
        Task<Users> GetUserByEmail(string Email); 
    }
}
