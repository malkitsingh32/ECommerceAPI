using Application.Handler.Categories.Commands.CreateUpdateCategory;
using Application.Handler.Categories.Queries.GetCategories;
using Application.Handler.Categories.Queries.GetCategoryById;
using DTO.Request;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : BaseController
    {
        [HttpGet]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var products = await Mediator.Send( new GetCategoriesQuery());
            return Ok(products);
        }

        [HttpPost]
        [Route("CreateUpdateCategory")]
        public async Task<IActionResult> CreateUpdateCategoryCreateUpdateCategory([FromBody] CreateUpdateCategoryDto createUpdateCategoryDto)
        {
            var products = await Mediator.Send(createUpdateCategoryDto.Adapt<CreateUpdateCategoryCommand>());
            return Ok(products);
        }

        [HttpPost]
        [Route("GetCategoryById")]
        public async Task<IActionResult> GetCategoryById([FromQuery] int categoryId)
        {
            var products = await Mediator.Send(new GetCategoryByIdQuery() { CategoryId = categoryId});
            return Ok(products);
        }
    }
}
