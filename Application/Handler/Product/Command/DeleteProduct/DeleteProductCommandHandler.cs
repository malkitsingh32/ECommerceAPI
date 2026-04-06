using Application.Abstraction.Services;
using Common.Common;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Handler.Product.Command.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, CommonResultResponseDto<string>>
    {
        private readonly IProductService _productService;

        public DeleteProductCommandHandler(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<CommonResultResponseDto<string>> Handle(DeleteProductCommand deleteProductCommand, CancellationToken cancellationToken)
        {
            return await _productService.DeleteProduct(deleteProductCommand.ProductId);
        }
    }
}
