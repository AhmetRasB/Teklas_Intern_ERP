namespace Teklas_Intern_ERP.DataAccess.DTOs
{
    public class MaterialCardDto
    {
        public int Id { get; set; }
        public string MaterialCode { get; set; }
        public string MaterialName { get; set; }
        public string MaterialType { get; set; }
        public int CategoryId { get; set; }
        public string UnitOfMeasure { get; set; }
        public string Barcode { get; set; }
        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public decimal PurchasePrice { get; set; }
        public decimal SalesPrice { get; set; }
        public decimal MinimumStockLevel { get; set; }
        public decimal MaximumStockLevel { get; set; }
        public decimal ReorderLevel { get; set; }
        public int? ShelfLife { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string Color { get; set; }
        public string OriginCountry { get; set; }
        public string Manufacturer { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }
    }
} 