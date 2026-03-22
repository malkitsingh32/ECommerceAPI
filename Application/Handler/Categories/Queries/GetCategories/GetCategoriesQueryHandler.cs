using Application.Abstraction.Services;
using Application.Handler.Categories.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Categories.Queries.GetCategories
{
    public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, CommonResultResponseDto<CategoryDto>>
    {
        private readonly ICategoryService _categoriesService;
        private readonly IRequestService _requestService;

        public GetCategoriesQueryHandler(ICategoryService categoriesService, IRequestService requestService)
        {
            _categoriesService = categoriesService;
            _requestService = requestService;
        }
        public async Task<CommonResultResponseDto<CategoryDto>> Handle(GetCategoriesQuery getCategoriesQuery, CancellationToken cancellationToken)
        {
            var requestBuilder = _requestService.GetRequestBuilder(getCategoriesQuery);
            return await _categoriesService.GetCategories(requestBuilder.GetPageIndex(), requestBuilder.GetPageSize(), requestBuilder.GetFilters(), requestBuilder.GetSorts(), getCategoriesQuery?.SearchText);
        }
    }
}
