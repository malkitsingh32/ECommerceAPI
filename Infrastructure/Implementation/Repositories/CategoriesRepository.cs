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

        public async Task<Tuple<int, List<Categories>>> GetCategories(int pageNumber, int pageSize, string whereClause, string sortQuery, string searchText = null)
        {
            int totalResult = 0;
            List<Categories> categories = new();

            using (var dbConnection = _dbContext.GetDbConnection())
            {
                using var result = await dbConnection.QueryMultipleAsync(
                    "usp_GetCategories",
                    _dbContext.GetDapperDynamicParameters(
                        _parameterManager.Get("PageNumber", pageNumber),
                        _parameterManager.Get("PageSize", pageSize),
                        _parameterManager.Get("WhereClause", whereClause),
                        _parameterManager.Get("SortQuery", sortQuery),
                        _parameterManager.Get("SearchText", searchText)
                    ),
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: 0);

                totalResult = result.Read<int>().FirstOrDefault();

                // Line 49 stays the same:
                categories = result.Read<Categories>().ToList();
            }

            return Tuple.Create(totalResult, categories);
        }

        public async Task<Categories> GetCategoryById(int categoryId)
        {
            return await _dbContext.ExecuteStoredProcedure<Categories>(
                "usp_GetCategoryById",
                _parameterManager.Get("CategoryId", categoryId));
        }
    }
}
