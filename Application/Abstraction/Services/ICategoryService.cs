using Application.Handler.Categories.Dtos;
using Common.Common;

namespace Application.Abstraction.Services
{
    public interface ICategoryService
    {
        Task<CommonResultResponseDto<List<CategoryListDto>>> GetCategories();
        Task<CommonResultResponseDto<int>> CreateUpdateCategory(CreateUpdateCategoryDto createUpdateCategory);
        Task<CommonResultResponseDto<CategoryListDto>> GetCategoryById(int CategoryId);
    }
}
