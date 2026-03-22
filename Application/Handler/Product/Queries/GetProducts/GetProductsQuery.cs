using Application.Handler.Common.Dtos;
using Application.Handler.Product.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Product.Queries.GetProducts
{
    public class GetProductsQuery : ServerRowRequest, IRequest<CommonResultResponseDto<ProductDto>>
    {
        public string? SearchText { get; set; }
    }
}
