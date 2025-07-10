using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    /// <summary>
    /// Bill of Material Item Data Transfer Object
    /// </summary>
    public sealed class BillOfMaterialItemDto
    {
        public long Id { get; set; }
        public long BillOfMaterialId { get; set; }
        public long MaterialCardId { get; set; }
        public int LineNumber { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = "EACH";
        public decimal ScrapFactor { get; set; } = 0;
        public string ComponentType { get; set; } = "RAW_MATERIAL";
        public bool IsOptional { get; set; } = false;
        public bool IsPhantom { get; set; } = false;
        public string IssueMethod { get; set; } = "MANUAL";
        public int? OperationSequence { get; set; }
        public DateTime? ValidFrom { get; set; }
        public DateTime? ValidTo { get; set; }
        public string? Description { get; set; }
        public decimal? CostAllocation { get; set; }
        public long? SupplierMaterialCardId { get; set; }
        public int? LeadTimeOffset { get; set; }

        // Navigation Properties
        public string? MaterialCardName { get; set; }
        public string? MaterialCardCode { get; set; }
        public string? SupplierMaterialCardName { get; set; }
        public string? SupplierMaterialCardCode { get; set; }

        // Audit Properties
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        // Display Properties
        public string ComponentTypeDisplay => ComponentType switch
        {
            "RAW_MATERIAL" => "Hammadde",
            "SEMI_FINISHED" => "Yarı Mamul",
            "FINISHED_GOOD" => "Mamul",
            "PACKAGING" => "Ambalaj",
            "CONSUMABLE" => "Sarf Malzeme",
            _ => ComponentType
        };

        public string IssueMethodDisplay => IssueMethod switch
        {
            "MANUAL" => "Manuel",
            "AUTOMATIC" => "Otomatik",
            "BACKFLUSH" => "Geri Besleme",
            _ => IssueMethod
        };

        public decimal TotalQuantity => Quantity * (1 + ScrapFactor / 100);
    }

    /// <summary>
    /// Bill of Material Item Validator
    /// </summary>
    public sealed class BillOfMaterialItemDtoValidator : AbstractValidator<BillOfMaterialItemDto>
    {
        public BillOfMaterialItemDtoValidator()
        {
            RuleFor(x => x.BillOfMaterialId)
                .GreaterThan(0)
                .WithMessage(Error.BillOfMaterialRequired);

            RuleFor(x => x.MaterialCardId)
                .GreaterThan(0)
                .WithMessage(Error.MaterialCardRequired);

            RuleFor(x => x.LineNumber)
                .GreaterThan(0)
                .WithMessage(Error.LineNumberMustBePositive);

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage(Error.QuantityMustBePositive);

            RuleFor(x => x.Unit)
                .NotEmpty()
                .WithMessage(Error.UnitRequired)
                .MaximumLength(10)
                .WithMessage(Error.UnitMaxLength);

            RuleFor(x => x.ScrapFactor)
                .GreaterThanOrEqualTo(0)
                .WithMessage(Error.ScrapFactorMustBeNonNegative)
                .LessThanOrEqualTo(100)
                .WithMessage(Error.ScrapFactorMaxValue);

            RuleFor(x => x.ComponentType)
                .NotEmpty()
                .WithMessage(Error.ComponentTypeRequired)
                .Must(x => new[] { "RAW_MATERIAL", "SEMI_FINISHED", "FINISHED_GOOD", "PACKAGING", "CONSUMABLE" }.Contains(x))
                .WithMessage(Error.InvalidComponentType);

            RuleFor(x => x.IssueMethod)
                .NotEmpty()
                .WithMessage(Error.IssueMethodRequired)
                .Must(x => new[] { "MANUAL", "AUTOMATIC", "BACKFLUSH" }.Contains(x))
                .WithMessage(Error.InvalidIssueMethod);

            RuleFor(x => x.OperationSequence)
                .GreaterThan(0)
                .When(x => x.OperationSequence.HasValue)
                .WithMessage(Error.OperationSequenceMustBePositive);

            RuleFor(x => x.ValidTo)
                .GreaterThan(x => x.ValidFrom)
                .When(x => x.ValidFrom.HasValue && x.ValidTo.HasValue)
                .WithMessage(Error.ValidToMustBeAfterValidFrom);

            RuleFor(x => x.Description)
                .MaximumLength(500)
                .WithMessage(Error.DescriptionMaxLength);

            RuleFor(x => x.CostAllocation)
                .GreaterThanOrEqualTo(0)
                .When(x => x.CostAllocation.HasValue)
                .WithMessage(Error.CostAllocationMustBeNonNegative)
                .LessThanOrEqualTo(100)
                .When(x => x.CostAllocation.HasValue)
                .WithMessage(Error.CostAllocationMaxValue);

            RuleFor(x => x.LeadTimeOffset)
                .GreaterThanOrEqualTo(0)
                .When(x => x.LeadTimeOffset.HasValue)
                .WithMessage(Error.LeadTimeOffsetMustBeNonNegative);
        }
    }
} 