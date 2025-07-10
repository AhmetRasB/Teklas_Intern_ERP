using System;
using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.UserManagement;

namespace Teklas_Intern_ERP.Entities.ProductionManagement
{
    /// <summary>
    /// Production Confirmation Entity - Enterprise Resource Planning
    /// Represents production reporting and confirmations
    /// </summary>
    public sealed class ProductionConfirmation : AuditEntity
    {
        /// <summary>
        /// Work order reference
        /// </summary>
        public long WorkOrderId { get; set; }

        /// <summary>
        /// Confirmation number/reference
        /// </summary>
        [Required]
        [StringLength(50)]
        public string ConfirmationNumber { get; set; } = string.Empty;

        /// <summary>
        /// Confirmation date and time
        /// </summary>
        public DateTime ConfirmationDate { get; set; }

        /// <summary>
        /// Confirmed good quantity
        /// </summary>
        public decimal ConfirmedQuantity { get; set; }

        /// <summary>
        /// Scrapped/rejected quantity
        /// </summary>
        public decimal ScrapQuantity { get; set; } = 0;

        /// <summary>
        /// Rework quantity
        /// </summary>
        public decimal ReworkQuantity { get; set; } = 0;

        /// <summary>
        /// Unit of measure
        /// </summary>
        [StringLength(10)]
        public string Unit { get; set; } = "EACH";

        /// <summary>
        /// Operator user who performed the work
        /// </summary>
        public long? OperatorUserId { get; set; }

        /// <summary>
        /// Work center/machine used
        /// </summary>
        [StringLength(100)]
        public string? WorkCenter { get; set; }

        /// <summary>
        /// Operation sequence number
        /// </summary>
        public int? OperationSequence { get; set; }

        /// <summary>
        /// Confirmation status (DRAFT, CONFIRMED, CANCELLED)
        /// </summary>
        [StringLength(20)]
        public new string Status { get; set; } = "DRAFT";

        /// <summary>
        /// Confirmation type (GOOD, SCRAP, REWORK, SETUP, MAINTENANCE)
        /// </summary>
        [StringLength(20)]
        public string ConfirmationType { get; set; } = "GOOD";

        /// <summary>
        /// Actual setup time in minutes
        /// </summary>
        public decimal? SetupTime { get; set; }

        /// <summary>
        /// Actual run time in minutes
        /// </summary>
        public decimal? RunTime { get; set; }

        /// <summary>
        /// Downtime in minutes
        /// </summary>
        public decimal? DownTime { get; set; }

        /// <summary>
        /// Downtime reason
        /// </summary>
        [StringLength(200)]
        public string? DownTimeReason { get; set; }

        /// <summary>
        /// Work shift
        /// </summary>
        [StringLength(50)]
        public string? Shift { get; set; }

        /// <summary>
        /// Production notes/comments
        /// </summary>
        [StringLength(500)]
        public string? Notes { get; set; }

        /// <summary>
        /// Quality check status (NOT_CHECKED, PASSED, FAILED, PENDING)
        /// </summary>
        [StringLength(20)]
        public string QualityStatus { get; set; } = "NOT_CHECKED";

        /// <summary>
        /// Quality notes
        /// </summary>
        [StringLength(500)]
        public string? QualityNotes { get; set; }

        /// <summary>
        /// Batch/Lot number
        /// </summary>
        [StringLength(50)]
        public string? BatchNumber { get; set; }

        /// <summary>
        /// Serial number start
        /// </summary>
        [StringLength(50)]
        public string? SerialNumberFrom { get; set; }

        /// <summary>
        /// Serial number end
        /// </summary>
        [StringLength(50)]
        public string? SerialNumberTo { get; set; }

        /// <summary>
        /// Cost center
        /// </summary>
        [StringLength(50)]
        public string? CostCenter { get; set; }

        /// <summary>
        /// Activity type (PRODUCTION, SETUP, MAINTENANCE, etc.)
        /// </summary>
        [StringLength(50)]
        public string ActivityType { get; set; } = "PRODUCTION";

        /// <summary>
        /// Production start time
        /// </summary>
        public DateTime? StartTime { get; set; }

        /// <summary>
        /// Production end time
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// Wait time in minutes
        /// </summary>
        public decimal? WaitTime { get; set; }

        /// <summary>
        /// Requires quality check
        /// </summary>
        public bool RequiresQualityCheck { get; set; } = false;

        /// <summary>
        /// Quality check result
        /// </summary>
        [StringLength(200)]
        public string? QualityCheckResult { get; set; }

        /// <summary>
        /// Posted by user ID
        /// </summary>
        public long? PostedByUserId { get; set; }

        /// <summary>
        /// Posted date
        /// </summary>
        public DateTime? PostedDate { get; set; }

        /// <summary>
        /// Reversal reason
        /// </summary>
        [StringLength(200)]
        public string? ReversalReason { get; set; }

        /// <summary>
        /// Actual material consumed flag
        /// </summary>
        public bool MaterialConsumed { get; set; } = false;

        /// <summary>
        /// Stock posting required
        /// </summary>
        public bool RequiresStockPosting { get; set; } = true;

        /// <summary>
        /// Stock posted flag
        /// </summary>
        public bool StockPosted { get; set; } = false;

        /// <summary>
        /// Stock posting date
        /// </summary>
        public DateTime? StockPostingDate { get; set; }

        /// <summary>
        /// Confirmed by user ID
        /// </summary>
        public long? ConfirmedByUserId { get; set; }

        /// <summary>
        /// Confirmation timestamp
        /// </summary>
        public DateTime? ConfirmedDate { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Work order navigation property
        /// </summary>
        public WorkOrder WorkOrder { get; set; } = null!;

        /// <summary>
        /// Operator user navigation property
        /// </summary>
        public User? OperatorUser { get; set; }

        /// <summary>
        /// Confirmed by user navigation property
        /// </summary>
        public User? ConfirmedByUser { get; set; }

        #endregion
    }
} 