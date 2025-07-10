namespace Teklas_Intern_ERP.DTOs
{
    /// <summary>
    /// Production Summary Data Transfer Object for Reporting
    /// </summary>
    public sealed class ProductionSummaryDto
    {
        public long WorkOrderId { get; set; }
        public string WorkOrderNumber { get; set; } = string.Empty;
        public decimal PlannedQuantity { get; set; }
        public decimal TotalConfirmed { get; set; }
        public decimal TotalScrap { get; set; }
        public decimal TotalRework { get; set; }
        public decimal CompletionPercentage { get; set; }
        public decimal YieldPercentage { get; set; }
        public int ConfirmationCount { get; set; }
        
        // Additional properties for service compatibility
        public decimal TotalQuantityProduced { get; set; }
        public decimal TotalScrapQuantity { get; set; }
        public decimal TotalReworkQuantity { get; set; }
        public int TotalConfirmations { get; set; }
        public decimal AverageEfficiency { get; set; }
        public decimal QualityRate { get; set; }
        public DateTime ReportDate { get; set; }
    }
} 