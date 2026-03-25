using Application.Abstraction.Repositories;
using Application.Abstraction.Services;
using Application.Handler.Categories.Dtos;
using Common.Common;
using Domain.Entities;
using Mapster;

namespace Infrastructure.Implementation.Services
{
    public class CategoriesService : ICategoryService
    {
        private readonly ICategoriesRepositories _categoriesRepositories;

        public CategoriesService(ICategoriesRepositories categoriesRepositories)
        {
            _categoriesRepositories = categoriesRepositories;
        }
        public async Task<CommonResultResponseDto<int>> CreateUpdateCategory(CreateUpdateCategoryDto createUpdateCategory)
        {
            var categoryId = await _categoriesRepositories.CreateUpdateCategories(createUpdateCategory.Adapt<Categories>());
            if (categoryId > 0)
            {
                return CommonResultResponseDto<int>.Success(new[] { "Category created/updated successfully." }, categoryId);
            }
            else
            {
                return CommonResultResponseDto<int>.Failure(new[] { "Failed to create/update category." }, categoryId);
            }
        }

        public async Task<CommonResultResponseDto<List<CategoryListDto>>> GetCategories()
        {
            var categories = await _categoriesRepositories.GetCategories();
            if (categories != null)
            {
                return CommonResultResponseDto<List<CategoryListDto>>.Success(new[] { "Categories retrieved successfully." }, categories.Adapt<List<CategoryListDto>>());
            }
            else
            {
                return CommonResultResponseDto<List<CategoryListDto>>.Failure(new[] { "Failed to retrieve categories." }, null);
            }
        }

        public async Task<CommonResultResponseDto<CategoryListDto>> GetCategoryById(int CategoryId)
        {
            var category = await _categoriesRepositories.GetCategoryById(CategoryId);
            if (category != null)
            {
                var categoryDto = category.Adapt<CategoryDto>();
                return CommonResultResponseDto<CategoryListDto>.Success(new[] { "Category retrieved successfully." }, category.Adapt<CategoryListDto>());
            }
            else
            {
                return CommonResultResponseDto<CategoryListDto>.Failure(new[] { "Failed to retrieve category." }, null);
            }
        }
    }
}
