using Application.Abstraction.DataBase;
using Application.Abstraction.Repositories;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Implementation.Repositories
{
    internal class UserRepository : IUserRepository
    {
        private readonly IDbContext _dbContext;
        private readonly IParameterManager _parameterManager;
        public UserRepository(IDbContext dbContext, IParameterManager parameterManager)
        {
            _dbContext = dbContext;
            _parameterManager = parameterManager;
        }

        public async Task<int> InsertUser(Users user, byte[] passwordHash, byte[] passwordSalt)
        {
            return await _dbContext.ExecuteStoredProcedure<int>("usp_InsertUser",
             _parameterManager.Get("@UserName", user.UserName),
             _parameterManager.Get("@FirstName", user.FirstName),
             _parameterManager.Get("@LastName", user.LastName),
             _parameterManager.Get("@PasswordHash", passwordHash, ParameterDirection.Input, DbType.Binary),
             _parameterManager.Get("@PasswordSalt", passwordSalt, ParameterDirection.Input, DbType.Binary),
             _parameterManager.Get("@Phone", user.Phone),
             _parameterManager.Get("@Email", user.Email),
             _parameterManager.Get("@Company", user.Company));
        }
        public async Task<Users> GetUserByUserId(int userId)
        {
            return await _dbContext.ExecuteStoredProcedure<Users>("usp_GetUserById", 
              _parameterManager.Get("@Id", userId)
              );
        }

        public async Task<Users> GetUserByEmail(string Email)
        {
            return await _dbContext.ExecuteStoredProcedure<Users>("usp_GetUserByEmail",
              _parameterManager.Get("@Email", Email)
              );
        }
    }
}
