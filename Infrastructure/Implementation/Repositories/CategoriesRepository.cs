using Application.Abstraction.DataBase;
using Application.Abstraction.Repositories;
using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Implementation.Repositories
{
    public class CategoriesRepository : ICategoriesRepositories
    {
        private readonly IDbContext _dbContext;
        private readonly IParameterManager _parameterManager;

        public CategoriesRepository(IDbContext dbContext, IParameterManager parameterManager)
        {
            _dbContext = dbContext;
            _parameterManager = parameterManager;
        }
        public async Task<int> CreateUpdateCategories(Categories categories)
        {
            return await _dbContext.ExecuteStoredProcedure<int>(
                "usp_CreateUpdateCategories",
                    _parameterManager.Get("CategoryId", categories.CategoryId),
                    _parameterManager.Get("CategoryName", categories.CategoryName),
                    _parameterManager.Get("Description", categories.Description));
        }

        public async Task<IList<Categories>> GetCategories()
        {
            return await _dbContext.ExecuteStoredProcedureList<Categories>(
               "usp_GetCategories");
        }

        public async Task<Categories> GetCategoryById(int categoryId)
        {
            return await _dbContext.ExecuteStoredProcedure<Categories>(
                "usp_GetCategoryById",
                _parameterManager.Get("CategoryId", categoryId));
        }
    }
}
