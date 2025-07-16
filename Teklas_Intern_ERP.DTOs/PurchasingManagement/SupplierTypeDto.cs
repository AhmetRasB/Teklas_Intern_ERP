using FluentValidation;

namespace Teklas_Intern_ERP.DTOs.PurchasingManagement
{
    public sealed class SupplierTypeDto
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public sealed class SupplierTypeDtoValidator : AbstractValidator<SupplierTypeDto>
    {
        public SupplierTypeDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Supplier type code is required")
                .MinimumLength(2).WithMessage("Supplier type code must be at least 2 characters")
                .MaximumLength(20).WithMessage("Supplier type code cannot exceed 20 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Supplier type name is required")
                .MinimumLength(2).WithMessage("Supplier type name must be at least 2 characters")
                .MaximumLength(200).WithMessage("Supplier type name cannot exceed 200 characters");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
} 