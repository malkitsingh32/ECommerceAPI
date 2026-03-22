using Application.Abstraction.Services;
using Application.Handler.Product.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Product.Queries.GetProductById
{
    public class GetProductByIdQueryHandler : IRequestHandler<GetProductByIdQuery, CommonResultResponseDto<ProductDto>>
    {
        private readonly IProductService _productService;

        public GetProductByIdQueryHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<CommonResultResponseDto<ProductDto>> Handle(GetProductByIdQuery getProductByIdQuery, CancellationToken cancellationToken)
        {
            return await _productService.GetProductById(getProductByIdQuery.ProductId);
        }
    }
}
