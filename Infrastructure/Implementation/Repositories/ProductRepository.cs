using Application.Abstraction.DataBase;
using Application.Abstraction.Repositories;
using Dapper;
using Domain.Entities;
using System.Data;

namespace Infrastructure.Implementation.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDbContext _dbContext;
        private readonly IParameterManager _parameterManager;

        public ProductRepository(IDbContext dbContext, IParameterManager parameterManager)
        {
            _dbContext = dbContext;
            _parameterManager = parameterManager;
        }

        public async Task<int> CreateUpdateProduct(Products products)
        {
            return await _dbContext.ExecuteStoredProcedure<int>("usp_CreateUpdateProduct",
               _parameterManager.Get("@ProductId", products.ProductId),
               _parameterManager.Get("@ProductName", products.ProductName),
               _parameterManager.Get("@Description", products.Description),
               _parameterManager.Get("@Sku", products.SKU),
               _parameterManager.Get("@CategoryId", products.CategoryId),
               _parameterManager.Get("@SellingPrice", products?.SellingPrice),
               _parameterManager.Get("@StockQuantity", products.StockQty),
               _parameterManager.Get("@UserId", products.CreatedBy));
                
        }

        public async Task<int> DeleteProduct(int productId)
        {
            return await _dbContext.ExecuteStoredProcedure<int>("usp_DeleteProduct",
               _parameterManager.Get("@ProductId", productId));
        }

        public async Task<Products> GetProductById(int productId)
        {
            return await _dbContext.ExecuteStoredProcedure<Products>("usp_GetProductById",
                _parameterManager.Get("@ProductId", productId));
        }

        public async Task<Tuple<int, List<Products>>> GetProducts(int pageNumber, int pageSize, string whereClause, string sortQuery, string searchText = null)
        {
            int totalResult = 0;
            List<Products> products = new();

            using (var dbConnection = _dbContext.GetDbConnection())
            {
                using var result = await dbConnection.QueryMultipleAsync(
                    "usp_GetProducts",
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
                products = result.Read<Products>().ToList();
            }

            return Tuple.Create(totalResult, products);
        }
    }
}
