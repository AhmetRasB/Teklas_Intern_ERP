using FluentValidation;

namespace Teklas_Intern_ERP.DTOs.WarehouseManagement
{
    public sealed class StockEntryDto
    {
        public long Id { get; set; }
        public string? EntryNumber { get; set; }
        public DateTime EntryDate { get; set; } = DateTime.UtcNow;
        public long? WarehouseId { get; set; }
        public long? LocationId { get; set; }
        public long? MaterialId { get; set; }
        public string? EntryType { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? ReferenceType { get; set; }
        public decimal Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalValue { get; set; }
        public string? BatchNumber { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public DateTime? ProductionDate { get; set; }
        public string? QualityStatus { get; set; }
        public string? Notes { get; set; }
        public string? EntryReason { get; set; }
        public string? ResponsiblePerson { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        // Navigation properties
        public string? WarehouseName { get; set; }
        public string? LocationName { get; set; }
        public string? MaterialName { get; set; }
        public string? MaterialCode { get; set; }
    }

    public sealed class StockEntryDtoValidator : AbstractValidator<StockEntryDto>
    {
        public StockEntryDtoValidator()
        {
            RuleFor(x => x.EntryNumber)
                .NotEmpty().WithMessage("Entry number is required")
                .MinimumLength(2).WithMessage("Entry number must be at least 2 characters")
                .MaximumLength(20).WithMessage("Entry number cannot exceed 20 characters");

            RuleFor(x => x.EntryDate)
                .NotEmpty().WithMessage("Entry date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Entry date cannot be in the future");

            RuleFor(x => x.WarehouseId)
                .NotNull().WithMessage("Warehouse is required")
                .GreaterThan(0).WithMessage("Warehouse is required");

            RuleFor(x => x.LocationId)
                .NotNull().WithMessage("Location is required")
                .GreaterThan(0).WithMessage("Location is required");

            RuleFor(x => x.MaterialId)
                .NotNull().WithMessage("Material is required")
                .GreaterThan(0).WithMessage("Material is required");

            RuleFor(x => x.EntryType)
                .NotEmpty().WithMessage("Entry type is required")
                .MaximumLength(50).WithMessage("Entry type cannot exceed 50 characters");

            RuleFor(x => x.ReferenceNumber)
                .MaximumLength(50).WithMessage("Reference number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.ReferenceNumber));

            RuleFor(x => x.ReferenceType)
                .MaximumLength(50).WithMessage("Reference type cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.ReferenceType));

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).When(x => x.UnitPrice.HasValue).WithMessage("Unit price must be positive");

            RuleFor(x => x.TotalValue)
                .GreaterThanOrEqualTo(0).When(x => x.TotalValue.HasValue).WithMessage("Total value must be positive");

            RuleFor(x => x.BatchNumber)
                .MaximumLength(50).WithMessage("Batch number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.BatchNumber));

            RuleFor(x => x.SerialNumber)
                .MaximumLength(50).WithMessage("Serial number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.SerialNumber));

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow).When(x => x.ExpiryDate.HasValue).WithMessage("Expiry date must be in the future");

            RuleFor(x => x.ProductionDate)
                .LessThanOrEqualTo(DateTime.UtcNow).When(x => x.ProductionDate.HasValue).WithMessage("Production date cannot be in the future");

            RuleFor(x => x.QualityStatus)
                .MaximumLength(50).WithMessage("Quality status cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.QualityStatus));

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));

            RuleFor(x => x.EntryReason)
                .MaximumLength(200).WithMessage("Entry reason cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.EntryReason));

            RuleFor(x => x.ResponsiblePerson)
                .MaximumLength(100).WithMessage("Responsible person cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.ResponsiblePerson));
        }
    }
} 