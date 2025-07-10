using System;
using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Entities.ProductionManagement
{
    /// <summary>
    /// Bill of Material Item Entity - Enterprise Resource Planning
    /// Represents individual components/materials within a BOM
    /// </summary>
    public sealed class BillOfMaterialItem : AuditEntity
    {
        /// <summary>
        /// Parent BOM ID
        /// </summary>
        public long BillOfMaterialId { get; set; }

        /// <summary>
        /// Component material card ID
        /// </summary>
        public long MaterialCardId { get; set; }

        /// <summary>
        /// Line number/sequence in BOM
        /// </summary>
        public int LineNumber { get; set; }

        /// <summary>
        /// Required quantity per base quantity
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit of measure
        /// </summary>
        [StringLength(10)]
        public string Unit { get; set; } = "EACH";

        /// <summary>
        /// Scrap factor percentage (0-100)
        /// </summary>
        public decimal ScrapFactor { get; set; } = 0;

        /// <summary>
        /// Component type (RAW_MATERIAL, SEMI_FINISHED, FINISHED_GOOD, PACKAGING)
        /// </summary>
        [StringLength(50)]
        public string ComponentType { get; set; } = "RAW_MATERIAL";

        /// <summary>
        /// Whether this component is optional
        /// </summary>
        public bool IsOptional { get; set; } = false;

        /// <summary>
        /// Phantom component (consumed directly without stock tracking)
        /// </summary>
        public bool IsPhantom { get; set; } = false;

        /// <summary>
        /// Issue method (MANUAL, AUTOMATIC, BACKFLUSH)
        /// </summary>
        [StringLength(20)]
        public string IssueMethod { get; set; } = "MANUAL";

        /// <summary>
        /// Operation sequence number
        /// </summary>
        public int? OperationSequence { get; set; }

        /// <summary>
        /// Valid from date
        /// </summary>
        public DateTime? ValidFrom { get; set; }

        /// <summary>
        /// Valid to date
        /// </summary>
        public DateTime? ValidTo { get; set; }

        /// <summary>
        /// Item description/notes
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Cost allocation percentage
        /// </summary>
        public decimal? CostAllocation { get; set; }

        /// <summary>
        /// Supplier material card ID (if different supplier)
        /// </summary>
        public long? SupplierMaterialCardId { get; set; }

        /// <summary>
        /// Lead time offset in days
        /// </summary>
        public int? LeadTimeOffset { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Parent BOM navigation property
        /// </summary>
        public BillOfMaterial BillOfMaterial { get; set; } = null!;

        /// <summary>
        /// Component material card navigation property
        /// </summary>
        public MaterialCard MaterialCard { get; set; } = null!;

        /// <summary>
        /// Supplier material card navigation property
        /// </summary>
        public MaterialCard? SupplierMaterialCard { get; set; }

        #endregion
    }
} 