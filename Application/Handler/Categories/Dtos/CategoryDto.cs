namespace Application.Handler.Categories.Dtos
{
    public class CategoryDto
    {
        public CategoryDto()
        {
            CategoryListDto = new List<CategoryListDto>();
        }
        public int TotalResults { get; set; }
        public  List<CategoryListDto> CategoryListDto { get; set; }
    }
    public class CategoryListDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
