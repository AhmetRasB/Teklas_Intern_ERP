using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.PurchasingManagement
{
    /// <summary>
    /// Purchase Order Item Entity - Enterprise Resource Planning
    /// Represents individual items in purchase orders
    /// </summary>
    public class PurchaseOrderItem : AuditEntity
    {
        /// <summary>
        /// Purchase order foreign key
        /// </summary>
        public long PurchaseOrderId { get; set; }

        /// <summary>
        /// Material foreign key
        /// </summary>
        public long MaterialId { get; set; }

        /// <summary>
        /// Line number in the order
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Quantity ordered
        /// </summary>
        [Required]
        public decimal Quantity { get; set; }

        /// <summary>
        /// Quantity received
        /// </summary>
        public decimal? QuantityReceived { get; set; }

        /// <summary>
        /// Unit price
        /// </summary>
        public decimal? UnitPrice { get; set; }

        /// <summary>
        /// Discount percentage
        /// </summary>
        public decimal? DiscountPercentage { get; set; }

        /// <summary>
        /// Discount amount
        /// </summary>
        public decimal? DiscountAmount { get; set; }

        /// <summary>
        /// Tax percentage
        /// </summary>
        public decimal? TaxPercentage { get; set; }

        /// <summary>
        /// Tax amount
        /// </summary>
        public decimal? TaxAmount { get; set; }

        /// <summary>
        /// Line total amount
        /// </summary>
        public decimal? LineTotal { get; set; }

        /// <summary>
        /// Expected delivery date for this item
        /// </summary>
        public DateTime? ExpectedDeliveryDate { get; set; }

        /// <summary>
        /// Actual delivery date for this item
        /// </summary>
        public DateTime? ActualDeliveryDate { get; set; }

        /// <summary>
        /// Item status (Pending, Partially Received, Received, Cancelled, etc.)
        /// </summary>
        [StringLength(50)]
        public string? ItemStatus { get; set; }

        /// <summary>
        /// Supplier's item code
        /// </summary>
        [StringLength(50)]
        public string? SupplierItemCode { get; set; }

        /// <summary>
        /// Item description
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Unit of measurement
        /// </summary>
        [StringLength(10)]
        public string? Unit { get; set; }

        /// <summary>
        /// Additional notes
        /// </summary>
        [StringLength(1000)]
        public string? Notes { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Purchase order navigation property
        /// </summary>
        public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;

        /// <summary>
        /// Material navigation property
        /// </summary>
        public virtual MaterialManagement.MaterialCard Material { get; set; } = null!;

        #endregion
    }
} 