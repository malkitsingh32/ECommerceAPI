using Domain.Entities;

namespace Application.Abstraction.Repositories
{
    public interface ICategoriesRepositories
    {
        Task<Tuple<int, List<Categories>>> GetCategories(int pageNumber, int pageSize, string whereClause, string sortQuery, string SearchText = null);
        Task<int> CreateUpdateCategories(Categories categories);
        Task<Categories> GetCategoryById(int categoryId);
    }
}
