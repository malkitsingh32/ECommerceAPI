using Domain.Entities;

namespace Application.Abstraction.Repositories
{
    public interface IProductRepository
    {
        Task<Tuple<int, List<Products>>> GetProducts(int pageNumber, int pageSize, string whereClause, string sortQuery, string SearchText = null);
        Task<int> CreateUpdateProduct(Products products);
        Task<Products> GetProductById(int productId);
        Task<int> DeleteProduct(int productId);
    }
}
