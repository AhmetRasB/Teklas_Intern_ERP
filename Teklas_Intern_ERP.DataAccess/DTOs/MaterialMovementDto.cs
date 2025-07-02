namespace Teklas_Intern_ERP.DataAccess.DTOs
{
    public class MaterialMovementDto
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public string MovementType { get; set; }
        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }
        public int? SourceWarehouseId { get; set; }
        public int? DestinationWarehouseId { get; set; }
        public DateTime MovementDate { get; set; }
        public string ReferenceDocumentNo { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
} 