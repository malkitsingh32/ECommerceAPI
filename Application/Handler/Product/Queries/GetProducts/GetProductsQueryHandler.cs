using Application.Abstraction.Services;
using Application.Handler.Product.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Product.Queries.GetProducts
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, CommonResultResponseDto<ProductDto>>
    {
        private readonly IRequestService _requestService;
        private readonly IProductService _productService;

        public GetProductsQueryHandler(IProductService productService, IRequestService requestService)
        {
            _requestService = requestService;
            _productService = productService;
        }
        public async Task<CommonResultResponseDto<ProductDto>> Handle(GetProductsQuery getProductsQuery, CancellationToken cancellationToken)
        {
            var requestBuilder = _requestService.GetRequestBuilder(getProductsQuery);
            return await _productService.GetProducts(requestBuilder.GetPageIndex(), requestBuilder.GetPageSize(), requestBuilder.GetFilters(), requestBuilder.GetSorts(), getProductsQuery?.SearchText);
            
        }
    }
}
