namespace Application.Handler.Product.Dtos
{
    public class CreateUpdateDto
    {
        public long? ProductId { get; set; }
        public string ProductName { get; set; }
        public string SKU { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public decimal SellingPrice { get; set; }
        public int StockQty { get; set; }
    }
}
