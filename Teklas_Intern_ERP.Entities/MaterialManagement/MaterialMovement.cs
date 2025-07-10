using System;
using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    /// <summary>
    /// Material Movement Entity - Enterprise Resource Planning
    /// Represents material stock movements (in/out transactions)
    /// </summary>
    public class MaterialMovement : AuditEntity
    {
        /// <summary>
        /// Material card foreign key
        /// </summary>
        public long MaterialCardId { get; set; }

        /// <summary>
        /// Movement type (IN, OUT, TRANSFER, ADJUSTMENT, etc.)
        /// </summary>
        [Required]
        [StringLength(50)]
        public string MovementType { get; set; } = string.Empty;

        /// <summary>
        /// Movement quantity (positive for IN, negative for OUT)
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price at the time of movement
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Total amount (quantity * unit price)
        /// </summary>
        public decimal? TotalAmount { get; set; }

        /// <summary>
        /// Movement date and time
        /// </summary>
        public DateTime MovementDate { get; set; }

        /// <summary>
        /// Reference number (PO, SO, Transfer, etc.)
        /// </summary>
        [StringLength(100)]
        public string? ReferenceNumber { get; set; }

        /// <summary>
        /// Reference type (PurchaseOrder, SalesOrder, Transfer, etc.)
        /// </summary>
        [StringLength(50)]
        public string? ReferenceType { get; set; }

        /// <summary>
        /// Warehouse/Location from
        /// </summary>
        [StringLength(100)]
        public string? LocationFrom { get; set; }

        /// <summary>
        /// Warehouse/Location to
        /// </summary>
        [StringLength(100)]
        public string? LocationTo { get; set; }

        /// <summary>
        /// Movement description/notes
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Responsible person/user
        /// </summary>
        [StringLength(100)]
        public string? ResponsiblePerson { get; set; }

        /// <summary>
        /// Supplier/Customer reference
        /// </summary>
        [StringLength(100)]
        public string? SupplierCustomer { get; set; }

        /// <summary>
        /// Batch/Lot number
        /// </summary>
        [StringLength(50)]
        public string? BatchNumber { get; set; }

        /// <summary>
        /// Serial number
        /// </summary>
        [StringLength(50)]
        public string? SerialNumber { get; set; }

        /// <summary>
        /// Expiry date for batch tracked items
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Stock balance after this movement
        /// </summary>
        public decimal? StockBalance { get; set; }

        /// <summary>
        /// Movement status (PENDING, COMPLETED, CANCELLED)
        /// </summary>
        [StringLength(20)]
        public new string? Status { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Material card navigation property
        /// </summary>
        public virtual MaterialCard MaterialCard { get; set; } = null!;

        #endregion
    }
} 