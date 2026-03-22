using Application.Handler.Categories.Dtos;
using Common.Common;

namespace Application.Abstraction.Services
{
    public interface ICategoryService
    {
        Task<CommonResultResponseDto<CategoryDto>> GetCategories(int pageNumber, int pageSize, string whereClause, string sortQuery, string SearchText = null);
        Task<CommonResultResponseDto<int>> CreateUpdateCategory(CreateUpdateCategoryDto createUpdateCategory);
        Task<CommonResultResponseDto<CategoryListDto>> GetCategoryById(int CategoryId);
    }
}
