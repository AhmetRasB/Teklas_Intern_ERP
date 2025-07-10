using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.Entities.UserManagement;

namespace Teklas_Intern_ERP.Entities.ProductionManagement
{
    /// <summary>
    /// Work Order Entity - Enterprise Resource Planning
    /// Represents production orders for manufacturing
    /// </summary>
    public sealed class WorkOrder : AuditEntity
    {
        /// <summary>
        /// Work order unique number
        /// </summary>
        [Required]
        [StringLength(50)]
        public string WorkOrderNumber { get; set; } = string.Empty;

        /// <summary>
        /// BOM reference for this work order
        /// </summary>
        public long BillOfMaterialId { get; set; }

        /// <summary>
        /// Final product material card ID
        /// </summary>
        public long ProductMaterialCardId { get; set; }

        /// <summary>
        /// Planned production quantity
        /// </summary>
        public decimal PlannedQuantity { get; set; }

        /// <summary>
        /// Completed good quantity
        /// </summary>
        public decimal CompletedQuantity { get; set; } = 0;

        /// <summary>
        /// Scrapped quantity
        /// </summary>
        public decimal ScrapQuantity { get; set; } = 0;

        /// <summary>
        /// Unit of measure
        /// </summary>
        [StringLength(10)]
        public string Unit { get; set; } = "EACH";

        /// <summary>
        /// Work order status (CREATED, RELEASED, IN_PROGRESS, COMPLETED, CANCELLED, ON_HOLD)
        /// </summary>
        [StringLength(20)]
        public new string Status { get; set; } = "CREATED";

        /// <summary>
        /// Priority level (1=Highest, 5=Lowest)
        /// </summary>
        public int Priority { get; set; } = 3;

        /// <summary>
        /// Planned start date
        /// </summary>
        public DateTime PlannedStartDate { get; set; }

        /// <summary>
        /// Planned end date
        /// </summary>
        public DateTime PlannedEndDate { get; set; }

        /// <summary>
        /// Actual start date
        /// </summary>
        public DateTime? ActualStartDate { get; set; }

        /// <summary>
        /// Actual end date
        /// </summary>
        public DateTime? ActualEndDate { get; set; }

        /// <summary>
        /// Due date for completion
        /// </summary>
        public DateTime? DueDate { get; set; }

        /// <summary>
        /// Work order description/notes
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Customer order reference
        /// </summary>
        [StringLength(100)]
        public string? CustomerOrderReference { get; set; }

        /// <summary>
        /// Production line/work center
        /// </summary>
        [StringLength(100)]
        public string? WorkCenter { get; set; }

        /// <summary>
        /// Production shift
        /// </summary>
        [StringLength(50)]
        public string? Shift { get; set; }

        /// <summary>
        /// Responsible supervisor user ID
        /// </summary>
        public long? SupervisorUserId { get; set; }

        /// <summary>
        /// Planned setup time in minutes
        /// </summary>
        public decimal? PlannedSetupTime { get; set; }

        /// <summary>
        /// Planned run time in minutes
        /// </summary>
        public decimal? PlannedRunTime { get; set; }

        /// <summary>
        /// Actual setup time in minutes
        /// </summary>
        public decimal? ActualSetupTime { get; set; }

        /// <summary>
        /// Actual run time in minutes
        /// </summary>
        public decimal? ActualRunTime { get; set; }

        /// <summary>
        /// Work order type (PRODUCTION, ASSEMBLY, REWORK, MAINTENANCE)
        /// </summary>
        [StringLength(50)]
        public string WorkOrderType { get; set; } = "PRODUCTION";

        /// <summary>
        /// Source type (MANUAL, SALES_ORDER, FORECAST, STOCK_REPLENISHMENT)
        /// </summary>
        [StringLength(50)]
        public string? SourceType { get; set; }

        /// <summary>
        /// Source reference ID
        /// </summary>
        public long? SourceReferenceId { get; set; }

        /// <summary>
        /// Released date
        /// </summary>
        public DateTime? ReleasedDate { get; set; }

        /// <summary>
        /// Released by user ID
        /// </summary>
        public long? ReleasedByUserId { get; set; }

        /// <summary>
        /// Completion percentage (0-100)
        /// </summary>
        public decimal CompletionPercentage { get; set; } = 0;

        /// <summary>
        /// Quality check required
        /// </summary>
        public bool RequiresQualityCheck { get; set; } = false;

        /// <summary>
        /// Quality check status (NOT_REQUIRED, PENDING, PASSED, FAILED)
        /// </summary>
        [StringLength(20)]
        public string QualityStatus { get; set; } = "NOT_REQUIRED";

        #region Navigation Properties

        /// <summary>
        /// BOM navigation property
        /// </summary>
        public BillOfMaterial BillOfMaterial { get; set; } = null!;

        /// <summary>
        /// Product material card navigation property
        /// </summary>
        public MaterialCard ProductMaterialCard { get; set; } = null!;

        /// <summary>
        /// Supervisor user navigation property
        /// </summary>
        public User? SupervisorUser { get; set; }

        /// <summary>
        /// Released by user navigation property
        /// </summary>
        public User? ReleasedByUser { get; set; }

        /// <summary>
        /// Production confirmations for this work order
        /// </summary>
        public ICollection<ProductionConfirmation> ProductionConfirmations { get; set; } = new List<ProductionConfirmation>();

        #endregion
    }
} 