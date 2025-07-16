using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.SalesManagement
{
    /// <summary>
    /// Customer Order Item Entity - Enterprise Resource Planning
    /// Represents individual items in customer orders
    /// </summary>
    public class CustomerOrderItem : AuditEntity
    {
        /// <summary>
        /// Customer order foreign key
        /// </summary>
        public long CustomerOrderId { get; set; }

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
        /// Quantity shipped
        /// </summary>
        public decimal? QuantityShipped { get; set; }

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
        /// Item status (Pending, Partially Shipped, Shipped, Delivered, Cancelled, etc.)
        /// </summary>
        [StringLength(50)]
        public string? ItemStatus { get; set; }

        /// <summary>
        /// Customer's item code
        /// </summary>
        [StringLength(50)]
        public string? CustomerItemCode { get; set; }

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
        /// Customer order navigation property
        /// </summary>
        public virtual CustomerOrder CustomerOrder { get; set; } = null!;

        /// <summary>
        /// Material navigation property
        /// </summary>
        public virtual MaterialManagement.MaterialCard Material { get; set; } = null!;

        #endregion
    }
} 