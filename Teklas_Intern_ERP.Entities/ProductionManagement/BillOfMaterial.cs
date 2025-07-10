using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Entities.ProductionManagement
{
    /// <summary>
    /// Bill of Material Entity - Enterprise Resource Planning
    /// Represents material recipes/formulas for production
    /// </summary>
    public sealed class BillOfMaterial : AuditEntity
    {
        /// <summary>
        /// BOM unique code
        /// </summary>
        [Required]
        [StringLength(50)]
        public string BOMCode { get; set; } = string.Empty;

        /// <summary>
        /// BOM descriptive name
        /// </summary>
        [Required]
        [StringLength(200)]
        public string BOMName { get; set; } = string.Empty;

        /// <summary>
        /// Product material card ID (final product)
        /// </summary>
        public long ProductMaterialCardId { get; set; }

        /// <summary>
        /// BOM version for revision control
        /// </summary>
        [StringLength(20)]
        public string Version { get; set; } = "1.0";

        /// <summary>
        /// Base quantity for this BOM
        /// </summary>
        public decimal BaseQuantity { get; set; } = 1;

        /// <summary>
        /// Unit of measure for base quantity
        /// </summary>
        [StringLength(10)]
        public string Unit { get; set; } = "EACH";

        /// <summary>
        /// Active status - only one BOM version can be active per product
        /// </summary>
        public new bool IsActive { get; set; } = true;

        /// <summary>
        /// BOM type (PRODUCTION, ASSEMBLY, DISASSEMBLY)
        /// </summary>
        [StringLength(50)]
        public string BOMType { get; set; } = "PRODUCTION";

        /// <summary>
        /// Effective date from
        /// </summary>
        public DateTime? EffectiveFrom { get; set; }

        /// <summary>
        /// Effective date to
        /// </summary>
        public DateTime? EffectiveTo { get; set; }

        /// <summary>
        /// BOM description/notes
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Route/Operation sequence
        /// </summary>
        [StringLength(100)]
        public string? RouteCode { get; set; }

        /// <summary>
        /// Standard production time in minutes
        /// </summary>
        public decimal? StandardTime { get; set; }

        /// <summary>
        /// Setup time in minutes
        /// </summary>
        public decimal? SetupTime { get; set; }

        /// <summary>
        /// Approval status (DRAFT, APPROVED, OBSOLETE)
        /// </summary>
        [StringLength(20)]
        public string ApprovalStatus { get; set; } = "DRAFT";

        /// <summary>
        /// Approved by user ID
        /// </summary>
        public long? ApprovedByUserId { get; set; }

        /// <summary>
        /// Approval date
        /// </summary>
        public DateTime? ApprovalDate { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Product material card navigation property
        /// </summary>
        public MaterialCard ProductMaterialCard { get; set; } = null!;

        /// <summary>
        /// BOM items collection
        /// </summary>
        public ICollection<BillOfMaterialItem> BOMItems { get; set; } = new List<BillOfMaterialItem>();

        /// <summary>
        /// Work orders using this BOM
        /// </summary>
        public ICollection<WorkOrder> WorkOrders { get; set; } = new List<WorkOrder>();

        #endregion
    }
} 