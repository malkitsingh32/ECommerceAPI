using Application.Handler.Categories.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Categories.Queries.GetCategoryById
{
    public class GetCategoryByIdQuery : IRequest<CommonResultResponseDto<CategoryListDto>>
    {
        public int CategoryId { get; set; }
    }
}
