using System;
using System.Collections.Generic;

namespace Teklas_Intern_ERP.DTOs
{
    /// <summary>
    /// BOM Cost Calculation Result
    /// </summary>
    public sealed class BOMCostResult
    {
        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal OverheadCost { get; set; }
        public decimal TotalCost { get; set; }
        public decimal TotalMaterialCost { get; set; } // Added for compatibility
        public string Currency { get; set; } = "TRY";
        public DateTime CalculationDate { get; set; }
    }

    /// <summary>
    /// BOM Cost Trend DTO
    /// </summary>
    public sealed class BOMCostTrendDto
    {
        public DateTime Date { get; set; }
        public decimal TotalCost { get; set; }
        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal OverheadCost { get; set; }
    }

    /// <summary>
    /// BOM Explosion DTO
    /// </summary>
    public sealed class BOMExplosionDto
    {
        public int Level { get; set; }
        public long MaterialCardId { get; set; }
        public string MaterialCardCode { get; set; } = string.Empty;
        public string MaterialCardName { get; set; } = string.Empty;
        public decimal RequiredQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string ComponentType { get; set; } = string.Empty;
        public decimal UnitCost { get; set; }
        public decimal TotalCost { get; set; }
    }

    /// <summary>
    /// Work Order Cost Result
    /// </summary>
    public sealed class WorkOrderCostResult
    {
        public decimal MaterialCost { get; set; }
        public decimal LaborCost { get; set; }
        public decimal OverheadCost { get; set; }
        public decimal TotalCost { get; set; }
        public string Currency { get; set; } = "TRY";
        public DateTime CalculationDate { get; set; }
    }

    /// <summary>
    /// Work Order Validation Result
    /// </summary>
    public sealed class WorkOrderValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Work Order Completion Statistics
    /// </summary>
    public sealed class WorkOrderCompletionStatistics
    {
        public int TotalWorkOrders { get; set; }
        public int CompletedWorkOrders { get; set; }
        public int InProgressWorkOrders { get; set; }
        public int PendingWorkOrders { get; set; }
        public int CancelledWorkOrders { get; set; }
        public int OverdueWorkOrders { get; set; }
        public decimal AverageCompletionTime { get; set; }
        public decimal OnTimeDeliveryRate { get; set; }
        public decimal AverageLeadTime { get; set; }
    }

    /// <summary>
    /// Production Confirmation Validation Result
    /// </summary>
    public sealed class ProductionConfirmationValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// BOM Validation Result
    /// </summary>
    public sealed class BOMValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
    }

    /// <summary>
    /// Material Consumption DTO
    /// </summary>
    public sealed class MaterialConsumptionDto
    {
        public long MaterialCardId { get; set; }
        public decimal ConsumedQuantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string BatchNumber { get; set; } = string.Empty;
    }

    /// <summary>
    /// Quality Check DTO
    /// </summary>
    public sealed class QualityCheckDto
    {
        public string Status { get; set; } = string.Empty;
        public string Result { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// Labor Hours Result
    /// </summary>
    public sealed class LaborHoursResult
    {
        public decimal TotalHours { get; set; }
        public decimal ProductiveHours { get; set; }
        public decimal SetupHours { get; set; }
        public decimal DowntimeHours { get; set; }
        public decimal EfficiencyPercentage { get; set; }
        
        // Additional properties for compatibility
        public decimal RunHours { get; set; }
        public decimal WaitHours { get; set; }
        public decimal BreakHours { get; set; }
        public long ConfirmationId { get; set; }
        public List<LaborHoursDetailDto> Details { get; set; } = new List<LaborHoursDetailDto>();
    }

    /// <summary>
    /// Operator Efficiency DTO
    /// </summary>
    public sealed class OperatorEfficiencyDto
    {
        public long OperatorUserId { get; set; }
        public string OperatorName { get; set; } = string.Empty;
        public decimal TotalQuantityProduced { get; set; }
        public decimal TotalScrapQuantity { get; set; }
        public decimal EfficiencyPercentage { get; set; }
        public decimal QualityRate { get; set; }
        
        // Additional properties for compatibility
        public decimal TotalQuantity { get; set; }
        public decimal TotalTime { get; set; }
        public decimal AverageEfficiency { get; set; }
        public int ConfirmationCount { get; set; }
    }

    /// <summary>
    /// Scrap Analysis DTO
    /// </summary>
    public sealed class ScrapAnalysisDto
    {
        public string WorkCenter { get; set; } = string.Empty;
        public decimal TotalScrapQuantity { get; set; }
        public decimal ScrapRate { get; set; }
        public string MainScrapReason { get; set; } = string.Empty;
        
        // Additional properties for compatibility
        public string ProductName { get; set; } = string.Empty;
        public decimal TotalProduced { get; set; }
        public decimal TotalScrap { get; set; }
        public decimal ScrapPercentage { get; set; }
        public string? PrimaryReason { get; set; }
    }

    /// <summary>
    /// Production Trend DTO - Updated
    /// </summary>
    public sealed class ProductionTrendDto
    {
        public DateTime Date { get; set; }
        public string Period { get; set; } = string.Empty;
        public decimal TotalQuantity { get; set; }
        public decimal ProducedQuantity { get; set; }
        public decimal ScrapQuantity { get; set; }
        public decimal EfficiencyPercentage { get; set; }
        public int CompletedWorkOrders { get; set; }
    }

    /// <summary>
    /// Work Center Utilization DTO
    /// </summary>
    public sealed class WorkCenterUtilizationDto
    {
        public string WorkCenter { get; set; } = string.Empty;
        public decimal UtilizationPercentage { get; set; }
        public decimal AvailableHours { get; set; }
        public decimal UsedHours { get; set; }
        
        // Additional properties for compatibility
        public decimal TotalHours { get; set; }
        public decimal ProductiveHours { get; set; }
        public decimal WaitHours { get; set; }
        public decimal BreakHours { get; set; }
        public int WorkOrdersCompleted { get; set; }
    }

    /// <summary>
    /// Labor Hours Detail DTO
    /// </summary>
    public sealed class LaborHoursDetailDto
    {
        public DateTime Date { get; set; }
        public decimal Hours { get; set; }
        public string ActivityType { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // Additional properties for compatibility
        public string Activity { get; set; } = string.Empty;
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
} 