using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    /// <summary>
    /// Material Card Entity - Enterprise Resource Planning
    /// Represents material/product information in the system
    /// </summary>
    public class MaterialCard : AuditEntity
    {
        public MaterialCard()
        {
            MaterialMovements = new HashSet<MaterialMovement>();
        }

        /// <summary>
        /// Unique material code (SKU)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CardCode { get; set; } = string.Empty;

        /// <summary>
        /// Material name/description
        /// </summary>
        [Required]
        [StringLength(200)]
        public string CardName { get; set; } = string.Empty;

        /// <summary>
        /// Material type (Raw Material, Finished Product, etc.)
        /// </summary>
        [StringLength(50)]
        public string? CardType { get; set; }

        /// <summary>
        /// Material category foreign key
        /// </summary>
        public long MaterialCategoryId { get; set; }

        /// <summary>
        /// Unit of measurement (kg, pcs, m, etc.)
        /// </summary>
        [StringLength(10)]
        public string? Unit { get; set; }

        /// <summary>
        /// Purchase price
        /// </summary>
        public decimal? PurchasePrice { get; set; }

        /// <summary>
        /// Sales price
        /// </summary>
        public decimal? SalesPrice { get; set; }

        /// <summary>
        /// Current stock quantity
        /// </summary>
        public decimal? CurrentStock { get; set; }

        /// <summary>
        /// Minimum stock level for alerts
        /// </summary>
        public decimal? MinimumStockLevel { get; set; }

        /// <summary>
        /// Maximum stock level
        /// </summary>
        public decimal? MaximumStockLevel { get; set; }

        /// <summary>
        /// Reorder level for procurement
        /// </summary>
        public decimal? ReorderLevel { get; set; }

        /// <summary>
        /// Material weight
        /// </summary>
        public decimal? Weight { get; set; }

        /// <summary>
        /// Material volume
        /// </summary>
        public decimal? Volume { get; set; }

        /// <summary>
        /// Material length
        /// </summary>
        public decimal? Length { get; set; }

        /// <summary>
        /// Material width
        /// </summary>
        public decimal? Width { get; set; }

        /// <summary>
        /// Material height
        /// </summary>
        public decimal? Height { get; set; }

        /// <summary>
        /// Barcode for scanning
        /// </summary>
        [StringLength(100)]
        public string? Barcode { get; set; }

        /// <summary>
        /// Material brand
        /// </summary>
        [StringLength(100)]
        public string? Brand { get; set; }

        /// <summary>
        /// Material model
        /// </summary>
        [StringLength(100)]
        public string? Model { get; set; }

        /// <summary>
        /// Additional description
        /// </summary>
        [StringLength(1000)]
        public string? Description { get; set; }

        // TODO: Uncomment these fields after running database migration
        /*
        /// <summary>
        /// Material color
        /// </summary>
        [StringLength(50)]
        public string? Color { get; set; }

        /// <summary>
        /// Origin country of the material
        /// </summary>
        [StringLength(100)]
        public string? OriginCountry { get; set; }

        /// <summary>
        /// Material manufacturer
        /// </summary>
        [StringLength(200)]
        public string? Manufacturer { get; set; }

        /// <summary>
        /// Shelf life in days
        /// </summary>
        public int? ShelfLife { get; set; }
        */

        /// <summary>
        /// Storage location
        /// </summary>
        [StringLength(100)]
        public string? StorageLocation { get; set; }

        /// <summary>
        /// Material status (Active, Discontinued, etc.)
        /// </summary>
        [StringLength(50)]
        public new string? Status { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Material category navigation property
        /// </summary>
        public virtual MaterialCategory MaterialCategory { get; set; } = null!;

        /// <summary>
        /// Material movements collection
        /// </summary>
        public virtual ICollection<MaterialMovement> MaterialMovements { get; set; }

        #endregion
    }
} 