using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Implementation.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private static readonly List<Users> _users = new()
        {
            new Users
            {
                UserId = 1,
                UserName = "demo.user",
                FirstName = "Demo",
                LastName = "User",
                Email = "demo@local.test",
                Phone = "9999999999",
                Role = 1
            }
        };

        private static int _nextUserId = 2;

        public Task<int> InsertUser(Users user, byte[] passwordHash, byte[] passwordSalt)
        {
            user.UserId = _nextUserId++;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            _users.Add(user);
            return Task.FromResult(user.UserId.Value);
        }

        public Task<Users> GetUserByUserId(int userId)
        {
            var user = _users.FirstOrDefault(u => u.UserId == userId);
            return Task.FromResult(user);
        }

        public Task<Users> GetUserByEmail(string Email)
        {
            var user = _users.FirstOrDefault(u => string.Equals(u.Email, Email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(user);
        }

        public Task<bool> IsEmailExist(string email)
        {
            var exists = _users.Any(u => string.Equals(u.Email, email, StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(exists);
        }
    }
}
