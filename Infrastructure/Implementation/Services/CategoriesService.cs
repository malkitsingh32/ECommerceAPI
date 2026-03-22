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

        public async Task<CommonResultResponseDto<CategoryDto>> GetCategories(int pageNumber, int pageSize, string whereClause, string sortQuery, string SearchText = null)
        {
            var (result, catergory) = await _categoriesRepositories.GetCategories(pageNumber, pageSize, whereClause, sortQuery, SearchText);
            if (result > 0)
            {
                var categoryDtos = new CategoryDto
                {
                    TotalResults = result,
                    CategoryListDto = catergory.Adapt<List<CategoryListDto>>()
                };
                return CommonResultResponseDto<CategoryDto>.Success(new[] { "Categories retrieved successfully." }, categoryDtos);
            }
            else
            {
                return CommonResultResponseDto<CategoryDto>.Failure(new[] { "Failed to retrieve categories." }, null);
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
