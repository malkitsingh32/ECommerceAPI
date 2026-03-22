using Common.Common;
using MediatR;

namespace Application.Handler.Categories.Commands.CreateUpdateCategory
{
    public class CreateUpdateCategoryCommand : IRequest<CommonResultResponseDto<int>>
    {
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
