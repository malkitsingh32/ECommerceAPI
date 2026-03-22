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
        [HttpPost]
        [Route("GetCategories")]
        public async Task<IActionResult> GetCategories([FromBody] GetCategoriesRequestDto getCategoriesRequestDto)
        {
            var products = await Mediator.Send(getCategoriesRequestDto.Adapt<GetCategoriesQuery>());
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
