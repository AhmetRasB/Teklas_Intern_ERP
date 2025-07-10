using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    /// <summary>
    /// Work Order Data Transfer Object
    /// </summary>
    public sealed class WorkOrderDto
    {
        public long Id { get; set; }
        public string WorkOrderNumber { get; set; } = string.Empty;
        public long BillOfMaterialId { get; set; }
        public long ProductMaterialCardId { get; set; }
        public decimal PlannedQuantity { get; set; }
        public decimal CompletedQuantity { get; set; } = 0;
        public decimal ScrapQuantity { get; set; } = 0;
        public string Unit { get; set; } = "EACH";
        public string Status { get; set; } = "CREATED";
        public int Priority { get; set; } = 3;
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Description { get; set; }
        public string? CustomerOrderReference { get; set; }
        public string? WorkCenter { get; set; }
        public string? Shift { get; set; }
        public long? SupervisorUserId { get; set; }
        public decimal? PlannedSetupTime { get; set; }
        public decimal? PlannedRunTime { get; set; }
        public decimal? ActualSetupTime { get; set; }
        public decimal? ActualRunTime { get; set; }
        public string WorkOrderType { get; set; } = "PRODUCTION";
        public string? SourceType { get; set; }
        public long? SourceReferenceId { get; set; }
        public DateTime? ReleasedDate { get; set; }
        public long? ReleasedByUserId { get; set; }
        public decimal CompletionPercentage { get; set; } = 0;
        public bool RequiresQualityCheck { get; set; } = false;
        public string QualityStatus { get; set; } = "NOT_REQUIRED";

        // Navigation Properties (Display)
        public string? BOMCode { get; set; }
        public string? BOMName { get; set; }
        public string? ProductMaterialCardName { get; set; }
        public string? ProductMaterialCardCode { get; set; }
        public string? SupervisorUserName { get; set; }
        public string? ReleasedByUserName { get; set; }

        // Computed Properties
        public string StatusDisplay => GetStatusDisplay(Status);
        public string PriorityDisplay => GetPriorityDisplay(Priority);
        public string WorkOrderTypeDisplay => GetWorkOrderTypeDisplay(WorkOrderType);
        public string SourceTypeDisplay => GetSourceTypeDisplay(SourceType);
        public string QualityStatusDisplay => GetQualityStatusDisplay(QualityStatus);
        
        public decimal RemainingQuantity => PlannedQuantity - CompletedQuantity - ScrapQuantity;
        public decimal TotalCompletedQuantity => CompletedQuantity + ScrapQuantity;
        public decimal YieldPercentage => PlannedQuantity > 0 ? (CompletedQuantity / PlannedQuantity) * 100 : 0;
        public decimal ScrapPercentage => PlannedQuantity > 0 ? (ScrapQuantity / PlannedQuantity) * 100 : 0;
        
        public bool IsOverdue => DueDate.HasValue && DueDate.Value < DateTime.Now && Status != "COMPLETED" && Status != "CANCELLED";
        public bool IsStarted => ActualStartDate.HasValue;
        public bool IsCompleted => Status == "COMPLETED";
        public bool CanStart => Status == "RELEASED";
        public bool CanComplete => Status == "IN_PROGRESS" && RemainingQuantity <= 0;
        
        public int PlannedDurationDays => (PlannedEndDate - PlannedStartDate).Days;
        public int? ActualDurationDays => ActualStartDate.HasValue && ActualEndDate.HasValue 
            ? (ActualEndDate.Value - ActualStartDate.Value).Days : null;

        // Audit Properties
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }
        public string? CreatedByUserName { get; set; }
        public string? UpdatedByUserName { get; set; }

        private static string GetStatusDisplay(string status)
        {
            return status switch
            {
                "CREATED" => "Oluşturuldu",
                "RELEASED" => "Serbest Bırakıldı",
                "IN_PROGRESS" => "Devam Ediyor",
                "COMPLETED" => "Tamamlandı",
                "CANCELLED" => "İptal Edildi",
                "ON_HOLD" => "Beklemede",
                _ => status
            };
        }

        private static string GetPriorityDisplay(int priority)
        {
            return priority switch
            {
                1 => "En Yüksek",
                2 => "Yüksek",
                3 => "Normal",
                4 => "Düşük",
                5 => "En Düşük",
                _ => priority.ToString()
            };
        }

        private static string GetWorkOrderTypeDisplay(string workOrderType)
        {
            return workOrderType switch
            {
                "PRODUCTION" => "Üretim",
                "ASSEMBLY" => "Montaj",
                "REWORK" => "Yeniden İşleme",
                "MAINTENANCE" => "Bakım",
                "QUALITY_CHECK" => "Kalite Kontrolü",
                _ => workOrderType
            };
        }

        private static string GetSourceTypeDisplay(string? sourceType)
        {
            return sourceType switch
            {
                "MANUAL" => "Manuel",
                "SALES_ORDER" => "Satış Siparişi",
                "FORECAST" => "Tahmin",
                "STOCK_REPLENISHMENT" => "Stok Tamamlama",
                "PLANNING" => "Planlama",
                null => "Belirtilmemiş",
                _ => sourceType
            };
        }

        private static string GetQualityStatusDisplay(string qualityStatus)
        {
            return qualityStatus switch
            {
                "NOT_REQUIRED" => "Gerekli Değil",
                "PENDING" => "Bekliyor",
                "PASSED" => "Geçti",
                "FAILED" => "Başarısız",
                "IN_PROGRESS" => "Devam Ediyor",
                _ => qualityStatus
            };
        }
    }

    /// <summary>
    /// WorkOrder DTO Validator
    /// </summary>
    public sealed class WorkOrderDtoValidator : AbstractValidator<WorkOrderDto>
    {
        public WorkOrderDtoValidator()
        {
            RuleFor(x => x.WorkOrderNumber)
                .NotEmpty().WithMessage(Error.WorkOrderNumberRequired)
                .MaximumLength(50).WithMessage(Error.WorkOrderNumberMaxLength);

            RuleFor(x => x.BillOfMaterialId)
                .GreaterThan(0).WithMessage(Error.BillOfMaterialRequired);

            RuleFor(x => x.ProductMaterialCardId)
                .GreaterThan(0).WithMessage(Error.ProductMaterialCardRequired);

            RuleFor(x => x.PlannedQuantity)
                .GreaterThan(0).WithMessage(Error.PlannedQuantityMustBePositive);

            RuleFor(x => x.CompletedQuantity)
                .GreaterThanOrEqualTo(0).WithMessage(Error.CompletedQuantityMustBeNonNegative)
                .LessThanOrEqualTo(x => x.PlannedQuantity).WithMessage(Error.CompletedQuantityCannotExceedPlanned);

            RuleFor(x => x.ScrapQuantity)
                .GreaterThanOrEqualTo(0).WithMessage(Error.ScrapQuantityMustBeNonNegative);

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage(Error.UnitRequired)
                .MaximumLength(10).WithMessage(Error.UnitMaxLength);

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage(Error.StatusRequired)
                .Must(BeValidStatus).WithMessage(Error.InvalidWorkOrderStatus);

            RuleFor(x => x.Priority)
                .InclusiveBetween(1, 5).WithMessage(Error.PriorityMustBeBetween1And5);

            RuleFor(x => x.PlannedEndDate)
                .GreaterThan(x => x.PlannedStartDate).WithMessage(Error.PlannedEndDateMustBeAfterStartDate);

            RuleFor(x => x.ActualEndDate)
                .GreaterThan(x => x.ActualStartDate)
                .When(x => x.ActualStartDate.HasValue && x.ActualEndDate.HasValue)
                .WithMessage(Error.ActualEndDateMustBeAfterStartDate);

            RuleFor(x => x.DueDate)
                .GreaterThanOrEqualTo(x => x.PlannedStartDate)
                .When(x => x.DueDate.HasValue)
                .WithMessage(Error.DueDateMustBeAfterPlannedStartDate);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(Error.DescriptionMaxLength);

            RuleFor(x => x.CustomerOrderReference)
                .MaximumLength(100).WithMessage(Error.CustomerOrderReferenceMaxLength);

            RuleFor(x => x.WorkCenter)
                .MaximumLength(100).WithMessage(Error.WorkCenterMaxLength);

            RuleFor(x => x.Shift)
                .MaximumLength(50).WithMessage(Error.ShiftMaxLength);

            RuleFor(x => x.PlannedSetupTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.PlannedSetupTime.HasValue)
                .WithMessage(Error.PlannedSetupTimeMustBeNonNegative);

            RuleFor(x => x.PlannedRunTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.PlannedRunTime.HasValue)
                .WithMessage(Error.PlannedRunTimeMustBeNonNegative);

            RuleFor(x => x.ActualSetupTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ActualSetupTime.HasValue)
                .WithMessage(Error.ActualSetupTimeMustBeNonNegative);

            RuleFor(x => x.ActualRunTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.ActualRunTime.HasValue)
                .WithMessage(Error.ActualRunTimeMustBeNonNegative);

            RuleFor(x => x.WorkOrderType)
                .NotEmpty().WithMessage(Error.WorkOrderTypeRequired)
                .Must(BeValidWorkOrderType).WithMessage(Error.InvalidWorkOrderType);

            RuleFor(x => x.SourceType)
                .Must(BeValidSourceType)
                .When(x => !string.IsNullOrEmpty(x.SourceType))
                .WithMessage(Error.InvalidSourceType);

            RuleFor(x => x.CompletionPercentage)
                .InclusiveBetween(0, 100).WithMessage(Error.CompletionPercentageMustBeBetween0And100);

            RuleFor(x => x.QualityStatus)
                .NotEmpty().WithMessage(Error.QualityStatusRequired)
                .Must(BeValidQualityStatus).WithMessage(Error.InvalidQualityStatus);

            // Business Rules
            RuleFor(x => x.ActualStartDate)
                .NotNull()
                .When(x => x.Status == "IN_PROGRESS" || x.Status == "COMPLETED")
                .WithMessage(Error.ActualStartDateRequiredForInProgressOrCompleted);

            RuleFor(x => x.ActualEndDate)
                .NotNull()
                .When(x => x.Status == "COMPLETED")
                .WithMessage(Error.ActualEndDateRequiredForCompleted);

            RuleFor(x => x)
                .Must(x => x.CompletedQuantity + x.ScrapQuantity <= x.PlannedQuantity)
                .WithMessage(Error.TotalCompletedCannotExceedPlanned);
        }

        private static bool BeValidStatus(string status)
        {
            var validStatuses = new[] { "CREATED", "RELEASED", "IN_PROGRESS", "COMPLETED", "CANCELLED", "ON_HOLD" };
            return validStatuses.Contains(status);
        }

        private static bool BeValidWorkOrderType(string workOrderType)
        {
            var validTypes = new[] { "PRODUCTION", "ASSEMBLY", "REWORK", "MAINTENANCE", "QUALITY_CHECK" };
            return validTypes.Contains(workOrderType);
        }

        private static bool BeValidSourceType(string? sourceType)
        {
            if (string.IsNullOrEmpty(sourceType)) return true;
            var validTypes = new[] { "MANUAL", "SALES_ORDER", "FORECAST", "STOCK_REPLENISHMENT", "PLANNING" };
            return validTypes.Contains(sourceType);
        }

        private static bool BeValidQualityStatus(string qualityStatus)
        {
            var validStatuses = new[] { "NOT_REQUIRED", "PENDING", "PASSED", "FAILED", "IN_PROGRESS" };
            return validStatuses.Contains(qualityStatus);
        }
    }
} 