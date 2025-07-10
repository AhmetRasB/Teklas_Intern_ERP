using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    /// <summary>
    /// Bill of Material Data Transfer Object
    /// </summary>
    public sealed class BillOfMaterialDto
    {
        public long Id { get; set; }
        public string BOMCode { get; set; } = string.Empty;
        public string BOMName { get; set; } = string.Empty;
        public long ProductMaterialCardId { get; set; }
        public string Version { get; set; } = "1.0";
        public decimal BaseQuantity { get; set; } = 1;
        public string Unit { get; set; } = "EACH";
        public bool IsActive { get; set; } = true;
        public string BOMType { get; set; } = "PRODUCTION";
        public DateTime? EffectiveFrom { get; set; }
        public DateTime? EffectiveTo { get; set; }
        public string? Description { get; set; }
        public string? RouteCode { get; set; }
        public decimal? StandardTime { get; set; }
        public decimal? SetupTime { get; set; }
        public string ApprovalStatus { get; set; } = "DRAFT";
        public long? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }

        // Navigation Properties (Display)
        public string? ProductMaterialCardName { get; set; }
        public string? ProductMaterialCardCode { get; set; }
        public string? ApprovedByUserName { get; set; }

        // Collection Properties
        public List<BillOfMaterialItemDto> BOMItems { get; set; } = new List<BillOfMaterialItemDto>();

        // Computed Properties
        public string BOMTypeDisplay => GetBOMTypeDisplay(BOMType);
        public string ApprovalStatusDisplay => GetApprovalStatusDisplay(ApprovalStatus);
        public bool IsExpired => EffectiveTo.HasValue && EffectiveTo.Value < DateTime.Now;
        public bool IsEffective => (!EffectiveFrom.HasValue || EffectiveFrom.Value <= DateTime.Now) && 
                                   (!EffectiveTo.HasValue || EffectiveTo.Value >= DateTime.Now);

        // Audit Properties
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public long? CreatedByUserId { get; set; }
        public long? UpdatedByUserId { get; set; }
        public string? CreatedByUserName { get; set; }
        public string? UpdatedByUserName { get; set; }

        private static string GetBOMTypeDisplay(string bomType)
        {
            return bomType switch
            {
                "PRODUCTION" => "Üretim",
                "ASSEMBLY" => "Montaj",
                "DISASSEMBLY" => "Demontaj",
                "PACKAGING" => "Ambalajlama",
                _ => bomType
            };
        }

        private static string GetApprovalStatusDisplay(string approvalStatus)
        {
            return approvalStatus switch
            {
                "DRAFT" => "Taslak",
                "APPROVED" => "Onaylandı",
                "OBSOLETE" => "Geçersiz",
                "PENDING" => "Beklemede",
                _ => approvalStatus
            };
        }
    }

    /// <summary>
    /// BillOfMaterial DTO Validator
    /// </summary>
    public sealed class BillOfMaterialDtoValidator : AbstractValidator<BillOfMaterialDto>
    {
        public BillOfMaterialDtoValidator()
        {
            RuleFor(x => x.BOMCode)
                .NotEmpty().WithMessage(Error.BOMCodeRequired)
                .MaximumLength(50).WithMessage(Error.BOMCodeMaxLength);

            RuleFor(x => x.BOMName)
                .NotEmpty().WithMessage(Error.BOMNameRequired)
                .MaximumLength(200).WithMessage(Error.BOMNameMaxLength);

            RuleFor(x => x.ProductMaterialCardId)
                .GreaterThan(0).WithMessage(Error.ProductMaterialCardRequired);

            RuleFor(x => x.Version)
                .MaximumLength(20).WithMessage(Error.BOMVersionMaxLength);

            RuleFor(x => x.BaseQuantity)
                .GreaterThan(0).WithMessage(Error.BaseQuantityMustBePositive);

            RuleFor(x => x.Unit)
                .NotEmpty().WithMessage(Error.UnitRequired)
                .MaximumLength(10).WithMessage(Error.UnitMaxLength);

            RuleFor(x => x.BOMType)
                .NotEmpty().WithMessage(Error.BOMTypeRequired)
                .Must(BeValidBOMType).WithMessage(Error.InvalidBOMType);

            RuleFor(x => x.EffectiveTo)
                .GreaterThan(x => x.EffectiveFrom)
                .When(x => x.EffectiveFrom.HasValue && x.EffectiveTo.HasValue)
                .WithMessage(Error.EffectiveToMustBeAfterEffectiveFrom);

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(Error.DescriptionMaxLength);

            RuleFor(x => x.RouteCode)
                .MaximumLength(100).WithMessage(Error.RouteCodeMaxLength);

            RuleFor(x => x.StandardTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.StandardTime.HasValue)
                .WithMessage(Error.StandardTimeMustBeNonNegative);

            RuleFor(x => x.SetupTime)
                .GreaterThanOrEqualTo(0)
                .When(x => x.SetupTime.HasValue)
                .WithMessage(Error.SetupTimeMustBeNonNegative);

            RuleFor(x => x.ApprovalStatus)
                .NotEmpty().WithMessage(Error.ApprovalStatusRequired)
                .Must(BeValidApprovalStatus).WithMessage(Error.InvalidApprovalStatus);

            RuleFor(x => x.BOMItems)
                .NotEmpty().WithMessage(Error.BOMItemsRequired)
                .When(x => x.ApprovalStatus == "APPROVED");

            RuleForEach(x => x.BOMItems).SetValidator(new BillOfMaterialItemDtoValidator());
        }

        private static bool BeValidBOMType(string bomType)
        {
            var validTypes = new[] { "PRODUCTION", "ASSEMBLY", "DISASSEMBLY", "PACKAGING" };
            return validTypes.Contains(bomType);
        }

        private static bool BeValidApprovalStatus(string approvalStatus)
        {
            var validStatuses = new[] { "DRAFT", "APPROVED", "OBSOLETE", "PENDING" };
            return validStatuses.Contains(approvalStatus);
        }
    }
} 