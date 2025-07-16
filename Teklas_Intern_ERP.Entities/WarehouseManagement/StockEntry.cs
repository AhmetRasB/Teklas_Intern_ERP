using System;
using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.WarehouseManagement
{
    /// <summary>
    /// Stock Entry Entity - Enterprise Resource Planning
    /// Represents stock entries in the system
    /// </summary>
    public class StockEntry : AuditEntity
    {
        /// <summary>
        /// Unique stock entry number
        /// </summary>
        [Required]
        [StringLength(20)]
        public string EntryNumber { get; set; } = string.Empty;

        /// <summary>
        /// Entry date
        /// </summary>
        [Required]
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Warehouse foreign key
        /// </summary>
        public long WarehouseId { get; set; }

        /// <summary>
        /// Location foreign key
        /// </summary>
        public long LocationId { get; set; }

        /// <summary>
        /// Material foreign key
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// Entry type (Purchase, Production, Return, Transfer, etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string EntryType { get; set; } = string.Empty;

        /// <summary>
        /// Reference document number (PO, WO, etc.)
        /// </summary>
        [StringLength(50)]
        public string? ReferenceNumber { get; set; }

        /// <summary>
        /// Reference document type
        /// </summary>
        [StringLength(50)]
        public string? ReferenceType { get; set; }

        /// <summary>
        /// Quantity entered
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Total value
        /// </summary>
        public decimal? TotalValue { get; set; }

        /// <summary>
        /// Batch number
        /// </summary>
        [StringLength(50)]
        public string? BatchNumber { get; set; }

        /// <summary>
        /// Serial number
        /// </summary>
        [StringLength(50)]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Expiry date
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Production date
        /// </summary>
        public DateTime? ProductionDate { get; set; }

        /// <summary>
        /// Quality status (Good, Damaged, Expired, etc.)
        /// </summary>
        [StringLength(50)]
        public string? QualityStatus { get; set; }

        /// <summary>
        /// Entry status (Pending, Completed, Cancelled, etc.)
        /// </summary>
        [StringLength(50)]
        public new string? Status { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        [StringLength(1000)]
        public string? Notes { get; set; }

        /// <summary>
        /// Entry reason
        /// </summary>
        [StringLength(200)]
        public string? EntryReason { get; set; }

        /// <summary>
        /// Responsible person
        /// </summary>
        [StringLength(100)]
        public string? ResponsiblePerson { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Warehouse navigation property
        /// </summary>
        public virtual Warehouse Warehouse { get; set; } = null!;

        /// <summary>
        /// Location navigation property
        /// </summary>
        public virtual Location Location { get; set; } = null!;

        /// <summary>
        /// Material navigation property
        /// </summary>
        public virtual MaterialManagement.MaterialCard Material { get; set; } = null!;

        #endregion
    }
} 