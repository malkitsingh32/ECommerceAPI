namespace DTO.Request
{
    public class GetCategoriesRequestDto : ServerRowRequest
    {
        public string? SearchText { get; set; }
    }
}
