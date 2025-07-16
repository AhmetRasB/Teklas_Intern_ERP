namespace Teklas_Intern_ERP.DTOs
{
    public class MaterialCardFilter
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? MaterialType { get; set; }
        public long? CategoryId { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public string? Manufacturer { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Barcode { get; set; }
        
        // Price Range Filters
        public decimal? MinPurchasePrice { get; set; }
        public decimal? MaxPurchasePrice { get; set; }
        public decimal? MinSalesPrice { get; set; }
        public decimal? MaxSalesPrice { get; set; }
        
        // Date Range Filters
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? UpdatedFrom { get; set; }
        public DateTime? UpdatedTo { get; set; }
        
        // Search Terms
        public string? SearchTerm { get; set; }
    }
} 