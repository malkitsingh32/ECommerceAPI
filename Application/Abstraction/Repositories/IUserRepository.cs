using Domain.Entities;

namespace Application.Abstraction.Repositories
{
    public interface IUserRepository
    {
        Task<int> InsertUser(Users user, byte[] passwordHash, byte[] passwordSalt);
        Task<bool> IsEmailExist(string email);
        Task<Users> GetUserByUserId(int userId);
        Task<Users> GetUserByEmail(string Email); 
    }
}
