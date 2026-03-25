using Application.Handler.Product.Command.CreateUpdtaeProduct;
using Application.Handler.Product.Queries.GetProductById;
using Application.Handler.Product.Queries.GetProducts;
using DTO.Request;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : BaseController
    {
        [HttpPost]
        [Route("GetProducts")]
        public async Task<IActionResult> GetProducts([FromBody] GetProductsDto getProductsDto)
        {
            var products = await Mediator.Send(getProductsDto.Adapt<GetProductsQuery>());
            return Ok(products);
        }

        [HttpPost]
        [Route("CreateUpdateProduct")]
        public async Task<IActionResult> CreateUpdateProducts([FromBody] CreateUpdateProductRequestDto createUpdateProductRequestDto)
        {
            var products = await Mediator.Send(createUpdateProductRequestDto.Adapt<CreateUpdateProductCommand>());
            return Ok(products);
        }

        [HttpGet]
        [Route("GetProductById")]
        public async Task<IActionResult> GetProductById([FromQuery] int productId)
        {
            var products = await Mediator.Send(new GetProductByIdQuery() { ProductId = productId });
            return Ok(products);
        }

    }
}
