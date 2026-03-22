namespace DTO.Request
{
    public class GetProductsDto : ServerRowRequest
    {
        public string? SearchText { get; set; }
    }
}
