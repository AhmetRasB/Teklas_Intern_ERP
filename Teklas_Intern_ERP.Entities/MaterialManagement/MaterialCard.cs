using System;
using System.ComponentModel.DataAnnotations.Schema;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    public class MaterialCard : AuditEntity
    {
        [Column("MaterialCode")]
        public string Code { get; set; }
        [Column("MaterialName")]
        public string Name { get; set; }
        public string? MaterialType { get; set; }
        public long CategoryId { get; set; }
        public MaterialCategory Category { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        [Precision(18,2)]
        public decimal? PurchasePrice { get; set; }
        [Precision(18,2)]
        public decimal? SalesPrice { get; set; }
        [Precision(18,2)]
        public decimal? MinimumStockLevel { get; set; }
        [Precision(18,2)]
        public decimal? MaximumStockLevel { get; set; }
        [Precision(18,2)]
        public decimal? ReorderLevel { get; set; }
        public int? ShelfLife { get; set; }
        [Precision(18,2)]
        public decimal? Weight { get; set; }
        [Precision(18,2)]
        public decimal? Volume { get; set; }
        [Precision(18,2)]
        public decimal? Length { get; set; }
        [Precision(18,2)]
        public decimal? Width { get; set; }
        [Precision(18,2)]
        public decimal? Height { get; set; }
        public string? Color { get; set; }
        public string? OriginCountry { get; set; }
        public string? Manufacturer { get; set; }
        public string? ManufacturerPartNumber { get; set; }
    }
} 