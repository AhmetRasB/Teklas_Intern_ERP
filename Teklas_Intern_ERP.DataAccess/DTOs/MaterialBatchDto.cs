namespace Teklas_Intern_ERP.DataAccess.DTOs
{
    public class MaterialBatchDto
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string BatchNo { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal Quantity { get; set; }
        public DateTime CreatedDate { get; set; }
    }
} 