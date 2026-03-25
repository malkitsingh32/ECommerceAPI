using Domain.Entities;

namespace Application.Abstraction.Repositories
{
    public interface ICategoriesRepositories
    {
        Task<IList<Categories>> GetCategories();
        Task<int> CreateUpdateCategories(Categories categories);
        Task<Categories> GetCategoryById(int categoryId);
    }
}
