using Application.Abstraction.Services;
using Application.Handler.Product.Command.CreateUpdtaeProduct;
using Application.Handler.Product.Dtos;
using Common.Common;
using Mapster;
using MediatR;

namespace Application.Handler.Product.Command.CreateUpdateProduct
{
    public class CreateUpdateProductCommandHandler : IRequestHandler<CreateUpdateProductCommand, CommonResultResponseDto<int>>
    {
        private readonly IProductService _productService;

        public CreateUpdateProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<CommonResultResponseDto<int>> Handle(CreateUpdateProductCommand createUpdateProductCommand, CancellationToken cancellationToken)
        {
            return await _productService.CreateUpdateProduct(createUpdateProductCommand.Adapt<CreateUpdateDto>());
        }
    }
}
