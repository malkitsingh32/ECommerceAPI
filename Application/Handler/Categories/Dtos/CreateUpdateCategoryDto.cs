namespace Application.Handler.Categories.Dtos
{
    public class CreateUpdateCategoryDto
    {
        public long? CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
