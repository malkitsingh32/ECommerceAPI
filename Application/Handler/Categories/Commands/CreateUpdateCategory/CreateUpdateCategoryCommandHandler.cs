using Application.Abstraction.Services;
using Common.Common;
using Mapster;
using MediatR;
using Application.Handler.Categories.Dtos;


namespace Application.Handler.Categories.Commands.CreateUpdateCategory
{
    public class CreateUpdateCategoryCommandHandler : IRequestHandler<CreateUpdateCategoryCommand, CommonResultResponseDto<int>>
    {
        private readonly ICategoryService _categoryService;

        public CreateUpdateCategoryCommandHandler(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        public async Task<CommonResultResponseDto<int>> Handle(CreateUpdateCategoryCommand createUpdateCategoryCommand, CancellationToken cancellationToken)
        {
            return await _categoryService.CreateUpdateCategory(createUpdateCategoryCommand.Adapt<CreateUpdateCategoryDto>());
        }
    }
}
