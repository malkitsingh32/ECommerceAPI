namespace Domain.Entities
{
    public class Products
    {
        public long? ProductId { get; set; }
        public string? ProductName { get; set; }
        public string? SKU { get; set; }
        public string? Description { get; set; }
        public long? CategoryId { get; set; }
        public decimal? SellingPrice { get; set; }
        public int StockQty { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
       
    }
}
