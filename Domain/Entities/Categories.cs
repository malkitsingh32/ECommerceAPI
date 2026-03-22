namespace Domain.Entities
{
    public class Categories
    {
        public long CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
