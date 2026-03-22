using Application.Handler.Product.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Product.Queries.GetProductById
{
    public class GetProductByIdQuery: IRequest<CommonResultResponseDto<ProductDto>>
    {
        public int ProductId { get; set; }
    }
}
