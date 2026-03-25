using Application.Abstraction.Services;
using Application.Handler.Categories.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CommonResultResponseDto<List<CategoryListDto>>>
    {
        private readonly ICategoryService _categoriesService;
        private readonly IRequestService _requestService;

        public GetCategoriesQueryHandler(ICategoryService categoriesService, IRequestService requestService)
        {
            _categoriesService = categoriesService;
            _requestService = requestService;
        }
        public async Task<CommonResultResponseDto<List<CategoryListDto>>> Handle(GetCategoriesQuery getCategoriesQuery, CancellationToken cancellationToken)
        {
            return await _categoriesService.GetCategories();
        }
    }
}
