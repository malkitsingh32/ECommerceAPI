using Application.Abstraction.Repositories;
using Domain.Entities;

namespace Infrastructure.Implementation.Repositories
{
    public class CategoriesRepository : ICategoriesRepositories
    {
        private static readonly List<Categories> _categories = new()
        {
            new Categories { CategoryId = 1, CategoryName = "Electronics", Description = "Electronic items", IsActive = true, IsDeleted = false, CreatedDate = DateTime.UtcNow },
            new Categories { CategoryId = 2, CategoryName = "Groceries", Description = "Daily grocery items", IsActive = true, IsDeleted = false, CreatedDate = DateTime.UtcNow },
            new Categories { CategoryId = 3, CategoryName = "Fashion", Description = "Clothing and accessories", IsActive = true, IsDeleted = false, CreatedDate = DateTime.UtcNow }
        };

        private static long _nextCategoryId = 4;

        public Task<int> CreateUpdateCategories(Categories categories)
        {
            if (categories.CategoryId <= 0)
            {
                categories.CategoryId = _nextCategoryId++;
                categories.CreatedDate = DateTime.UtcNow;
                categories.IsActive = true;
                categories.IsDeleted = false;
                _categories.Add(categories);
                return Task.FromResult((int)categories.CategoryId);
            }

            var existing = _categories.FirstOrDefault(c => c.CategoryId == categories.CategoryId);
            if (existing == null)
            {
                return Task.FromResult(0);
            }

            existing.CategoryName = categories.CategoryName;
            existing.Description = categories.Description;
            existing.UpdatedBy = categories.UpdatedBy;
            existing.UpdatedDate = DateTime.UtcNow;

            return Task.FromResult((int)existing.CategoryId);
        }

        public Task<IList<Categories>> GetCategories()
        {
            IList<Categories> result = _categories.Where(c => c.IsDeleted != true).ToList();
            return Task.FromResult(result);
        }

        public Task<Categories> GetCategoryById(int categoryId)
        {
            var category = _categories.FirstOrDefault(c => c.CategoryId == categoryId && c.IsDeleted != true);
            return Task.FromResult(category);
        }
    }
}
