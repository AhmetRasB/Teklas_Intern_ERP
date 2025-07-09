using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    [Table("MaterialCards")]

    public class MaterialCard : AuditEntity
    {
        [Required]
        [Column("MaterialCode")]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        [Column("MaterialName")]
        public string Name { get; set; } = string.Empty;
        
        public string? MaterialType { get; set; }
        
        [Required]
        [ForeignKey("Category")]
        public long CategoryId { get; set; }
        public MaterialCategory Category { get; set; } = null!;
        
        public string? UnitOfMeasure { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? PurchasePrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? SalesPrice { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MinimumStockLevel { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? MaximumStockLevel { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? ReorderLevel { get; set; }
        
        public int? ShelfLife { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Weight { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Volume { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Length { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Width { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal? Height { get; set; }
        
        public string? Color { get; set; }
        public string? OriginCountry { get; set; }
        public string? Manufacturer { get; set; }
        public string? ManufacturerPartNumber { get; set; }
        
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }
} 