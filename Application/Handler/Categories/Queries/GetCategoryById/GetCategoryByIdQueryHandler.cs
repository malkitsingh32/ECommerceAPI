using Application.Abstraction.Services;
using Application.Handler.Categories.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQueryHandler : IRequestHandler<GetCategoryByIdQuery, CommonResultResponseDto<CategoryListDto>>
    {
        private readonly ICategoryService _categoryService;

        public GetCategoryByIdQueryHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<CommonResultResponseDto<CategoryListDto>> Handle(GetCategoryByIdQuery getCategoryByIdQuery, CancellationToken cancellationToken)
        {
            return await _categoryService.GetCategoryById(getCategoryByIdQuery.CategoryId);
        }
    }
}
