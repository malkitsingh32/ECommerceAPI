using Application.Handler.Categories.Dtos;
using Application.Handler.Common.Dtos;
using Common.Common;
using MediatR;

namespace Application.Handler.Categories.Queries.GetCategories
{
    public class GetCategoriesQuery: IRequest<CommonResultResponseDto<List<CategoryListDto>>>
    {
    }
}
