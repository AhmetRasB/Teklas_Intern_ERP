using System;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    /// <summary>
    /// Production Confirmation Data Transfer Object
    /// </summary>
    public sealed class ProductionConfirmationDto
    {
        public long Id { get; set; }
        public long WorkOrderId { get; set; }
        public string ConfirmationNumber { get; set; } = string.Empty;
        public DateTime ConfirmationDate { get; set; }
        public decimal ConfirmedQuantity { get; set; }
        public decimal ScrapQuantity { get; set; } = 0;
        public decimal ReworkQuantity { get; set; } = 0;
        public string Unit { get; set; } = "EACH";
        public long? OperatorUserId { get; set; }
        public string? WorkCenter { get; set; }
        public int? OperationSequence { get; set; }
        public string Status { get; set; } = "DRAFT";
        public string ConfirmationType { get; set; } = "GOOD";
        public decimal? SetupTime { get; set; }
        public decimal? RunTime { get; set; }
        public decimal? DownTime { get; set; }
        public string? DownTimeReason { get; set; }
        public string? Shift { get; set; }
        public string? Notes { get; set; }
        public string QualityStatus { get; set; } = "NOT_CHECKED";
        public string? QualityNotes { get; set; }
        public string? BatchNumber { get; set; }
        public string? SerialNumberFrom { get; set; }
        public string? SerialNumberTo { get; set; }
        public string? CostCenter { get; set; }
        public string ActivityType { get; set; } = "PRODUCTION";
        public bool MaterialConsumed { get; set; } = false;
        public bool RequiresStockPosting { get; set; } = true;
        public bool StockPosted { get; set; } = false;
        public DateTime? StockPostingDate { get; set; }
        public long? ConfirmedByUserId { get; set; }
        public DateTime? ConfirmedDate { get; set; }

        // Navigation Properties (Display)
        public string? WorkOrderNumber { get; set; }
        public string? OperatorUserName { get; set; }
        public string? ConfirmedByUserName { get; set; }
        public string? ProductMaterialCardName { get; set; }
        public string? ProductMaterialCardCode { get; set; }

        // Computed Properties
        public string StatusDisplay => GetStatusDisplay(Status);
        public string ConfirmationTypeDisplay => GetConfirmationTypeDisplay(ConfirmationType);
        public string QualityStatusDisplay => GetQualityStatusDisplay(QualityStatus);
        
        public decimal TotalQuantity => ConfirmedQuantity + ScrapQuantity + ReworkQuantity;
        public decimal? TotalTime => (SetupTime ?? 0) + (RunTime ?? 0) + (DownTime ?? 0);
        public decimal? EfficiencyPercentage => TotalTime > 0 ? ((RunTime ?? 0) / TotalTime) * 100 : null;
        public decimal YieldPercentage => TotalQuantity > 0 ? (ConfirmedQuantity / TotalQuantity) * 100 : 0;
        public decimal ScrapPercentage => TotalQuantity > 0 ? (ScrapQuantity / TotalQuantity) * 100 : 0;
        public decimal ReworkPercentage => TotalQuantity > 0 ? (ReworkQuantity / TotalQuantity) * 100 : 0;
        
        public bool IsConfirmed => Status == "CONFIRMED";
        public bool IsDraft => Status == "DRAFT";
        public bool HasQualityIssues => QualityStatus == "FAILED" || ScrapQuantity > 0;
        public bool RequiresQualityCheck => QualityStatus != "NOT_CHECKED";
        public bool HasSerialNumbers => !string.IsNullOrEmpty(SerialNumberFrom) || !string.IsNullOrEmpty(SerialNumberTo);
        public bool HasBatch => !string.IsNullOrEmpty(BatchNumber);

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
                "DRAFT" => "Taslak",
                "CONFIRMED" => "Onaylandı",
                "CANCELLED" => "İptal Edildi",
                "POSTED" => "Kaydedildi",
                _ => status
            };
        }

        private static string GetConfirmationTypeDisplay(string confirmationType)
        {
            return confirmationType switch
            {
                "GOOD" => "İyi",
                "SCRAP" => "Hurda",
                "REWORK" => "Yeniden İşleme",
                "SETUP" => "Hazırlık",
                "MAINTENANCE" => "Bakım",
                "QUALITY_CHECK" => "Kalite Kontrolü",
                _ => confirmationType
            };
        }

        private static string GetQualityStatusDisplay(string qualityStatus)
        {
            return qualityStatus switch
            {
                "NOT_CHECKED" => "Kontrol Edilmedi",
                "PASSED" => "Geçti",
                "FAILED" => "Başarısız",
                "PENDING" => "Beklemede",
                _ => qualityStatus
            };
        }
    }

    /// <summary>
    /// ProductionConfirmation DTO Validator
    /// </summary>
    public sealed class ProductionConfirmationDtoValidator : AbstractValidator<ProductionConfirmationDto>
    {
        public ProductionConfirmationDtoValidator()
        {
            RuleFor(x => x.WorkOrderId)
                .GreaterThan(0).WithMessage(Error.WorkOrderRequired);

            RuleFor(x => x.ConfirmationNumber)
                .NotEmpty().WithMessage(Error.ConfirmationNumberRequired)
                .MaximumLength(50).WithMessage(Error.ConfirmationNumberMaxLength);

            RuleFor(x => x.ConfirmationDate)
                .NotEmpty().WithMessage(Error.ConfirmationDateRequired)
                .LessThanOrEqualTo(DateTime.Now).WithMessage(Error.ConfirmationDateCannotBeFuture);

            RuleFor(x => x.ConfirmedQuantity)
                .GreaterThanOrEqualTo(0).WithMessage(Error.ConfirmedQuantityMustBeNonNegative);

            RuleFor(x => x.ScrapQuantity)
                .GreaterThanOrEqualTo(0).WithMessage(Error.ScrapQuantityMustBeNonNegative);

            RuleFor(x => x.ReworkQuantity)
                .GreaterThanOrEqualTo(0).WithMessage(Error.ReworkQuantityMustBeNonNegative);

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage(Error.UnitRequired)
                .MaximumLength(10).WithMessage(Error.UnitMaxLength);

            RuleFor(x => x.WorkCenter)
                .MaximumLength(100).WithMessage(Error.WorkCenterMaxLength);

            RuleFor(x => x.OperationSequence)
                .GreaterThan(0)
                .When(x => x.OperationSequence.HasValue)
                .WithMessage(Error.OperationSequenceMustBePositive);

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage(Error.StatusRequired)
                .Must(BeValidStatus).WithMessage(Error.InvalidConfirmationStatus);

            RuleFor(x => x.ConfirmationType)
                .NotEmpty().WithMessage(Error.ConfirmationTypeRequired)
                .Must(BeValidConfirmationType).WithMessage(Error.InvalidConfirmationType);

            RuleFor(x => x.SetupTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SetupTime.HasValue)
                .WithMessage(Error.SetupTimeMustBeNonNegative);

            RuleFor(x => x.RunTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.RunTime.HasValue)
                .WithMessage(Error.RunTimeMustBeNonNegative);

            RuleFor(x => x.DownTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.DownTime.HasValue)
                .WithMessage(Error.DownTimeMustBeNonNegative);

            RuleFor(x => x.DownTimeReason)
                .NotEmpty()
                .When(x => x.DownTime.HasValue && x.DownTime.Value > 0)
                .WithMessage(Error.DownTimeReasonRequiredWhenDownTimeExists)
                .MaximumLength(200).WithMessage(Error.DownTimeReasonMaxLength);

            RuleFor(x => x.Shift)
                .MaximumLength(50).WithMessage(Error.ShiftMaxLength);

            RuleFor(x => x.Notes)
                .MaximumLength(500).WithMessage(Error.NotesMaxLength);

            RuleFor(x => x.QualityStatus)
                .NotEmpty().WithMessage(Error.QualityStatusRequired)
                .Must(BeValidQualityStatus).WithMessage(Error.InvalidQualityStatus);

            RuleFor(x => x.QualityNotes)
                .MaximumLength(500).WithMessage(Error.QualityNotesMaxLength);

            RuleFor(x => x.BatchNumber)
                .MaximumLength(50).WithMessage(Error.BatchNumberMaxLength);

            RuleFor(x => x.SerialNumberFrom)
                .MaximumLength(50).WithMessage(Error.SerialNumberMaxLength);

            RuleFor(x => x.SerialNumberTo)
                .MaximumLength(50).WithMessage(Error.SerialNumberMaxLength)
                .GreaterThanOrEqualTo(x => x.SerialNumberFrom)
                .When(x => !string.IsNullOrEmpty(x.SerialNumberFrom) && !string.IsNullOrEmpty(x.SerialNumberTo))
                .WithMessage(Error.SerialNumberToMustBeGreaterThanFrom);

            RuleFor(x => x.CostCenter)
                .MaximumLength(50).WithMessage(Error.CostCenterMaxLength);

            // Business Rules
            RuleFor(x => x)
                .Must(x => x.ConfirmedQuantity + x.ScrapQuantity + x.ReworkQuantity > 0)
                .WithMessage(Error.TotalQuantityMustBeGreaterThanZero);

            RuleFor(x => x.OperatorUserId)
                .NotNull()
                .When(x => x.Status == "CONFIRMED")
                .WithMessage(Error.OperatorRequiredForConfirmedProduction);

            RuleFor(x => x.ConfirmedByUserId)
                .NotNull()
                .When(x => x.Status == "CONFIRMED")
                .WithMessage(Error.ConfirmedByUserRequiredForConfirmedProduction);

            RuleFor(x => x.ConfirmedDate)
                .NotNull()
                .When(x => x.Status == "CONFIRMED")
                .WithMessage(Error.ConfirmedDateRequiredForConfirmedProduction);

            RuleFor(x => x.QualityNotes)
                .NotEmpty()
                .When(x => x.QualityStatus == "FAILED")
                .WithMessage(Error.QualityNotesRequiredForFailedQuality);
        }

        private static bool BeValidStatus(string status)
        {
            var validStatuses = new[] { "DRAFT", "CONFIRMED", "CANCELLED", "POSTED" };
            return validStatuses.Contains(status);
        }

        private static bool BeValidConfirmationType(string confirmationType)
        {
            var validTypes = new[] { "GOOD", "SCRAP", "REWORK", "SETUP", "MAINTENANCE", "QUALITY_CHECK" };
            return validTypes.Contains(confirmationType);
        }

        private static bool BeValidQualityStatus(string qualityStatus)
        {
            var validStatuses = new[] { "NOT_CHECKED", "PASSED", "FAILED", "PENDING", "IN_PROGRESS" };
            return validStatuses.Contains(qualityStatus);
        }
    }
} 