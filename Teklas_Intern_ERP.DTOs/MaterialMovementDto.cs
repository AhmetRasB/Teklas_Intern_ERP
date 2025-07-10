using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    public sealed class MaterialMovementDto
    {
        public long Id { get; set; }
        public long MaterialCardId { get; set; }
        public string? MovementType { get; set; }
        public decimal Quantity { get; set; }
        public decimal? UnitPrice { get; set; }
        public decimal? TotalAmount { get; set; }
        public DateTime MovementDate { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? ReferenceType { get; set; }
        public string? LocationFrom { get; set; }
        public string? LocationTo { get; set; }
        public string? Description { get; set; }
        public string? ResponsiblePerson { get; set; }
        public string? SupplierCustomer { get; set; }
        public string? BatchNumber { get; set; }
        public string? SerialNumber { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public decimal? StockBalance { get; set; }
        public string? Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public bool IsDeleted { get; set; }
        
        // Navigation Properties
        public string? MaterialCardName { get; set; }
        public string? MaterialCardCode { get; set; }
        
        // Computed Properties
        public string? MovementTypeDisplay { get; set; }
        public string? StatusDisplay { get; set; }
    }

    public sealed class MaterialMovementDtoValidator : AbstractValidator<MaterialMovementDto>
    {
        public MaterialMovementDtoValidator()
        {
            RuleFor(x => x.MaterialCardId)
                .NotEmpty().WithMessage(Error.MaterialCardRequired)
                .GreaterThan(0).WithMessage(Error.MaterialCardIdInvalid);

            RuleFor(x => x.MovementType)
                .NotEmpty().WithMessage(Error.MovementTypeRequired)
                .MaximumLength(50).WithMessage(Error.MovementTypeMaxLength)
                .Must(BeValidMovementType).WithMessage(Error.MovementTypeInvalid);

            RuleFor(x => x.Quantity)
                .NotEmpty().WithMessage(Error.QuantityRequired)
                .NotEqual(0).WithMessage(Error.QuantityCannotBeZero);

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage(Error.UnitPricePositive)
                .When(x => x.UnitPrice.HasValue);

            RuleFor(x => x.MovementDate)
                .NotEmpty().WithMessage(Error.MovementDateRequired)
                .LessThanOrEqualTo(DateTime.Now.AddDays(1)).WithMessage(Error.MovementDateFuture);

            RuleFor(x => x.ReferenceNumber)
                .MaximumLength(100).WithMessage(Error.ReferenceNumberMaxLength)
                .When(x => !string.IsNullOrEmpty(x.ReferenceNumber));

            RuleFor(x => x.ReferenceType)
                .MaximumLength(50).WithMessage(Error.ReferenceTypeMaxLength)
                .When(x => !string.IsNullOrEmpty(x.ReferenceType));

            RuleFor(x => x.LocationFrom)
                .MaximumLength(100).WithMessage(Error.LocationFromMaxLength)
                .When(x => !string.IsNullOrEmpty(x.LocationFrom));

            RuleFor(x => x.LocationTo)
                .MaximumLength(100).WithMessage(Error.LocationToMaxLength)
                .When(x => !string.IsNullOrEmpty(x.LocationTo));

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(Error.DescriptionMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.ResponsiblePerson)
                .MaximumLength(100).WithMessage(Error.ResponsiblePersonMaxLength)
                .When(x => !string.IsNullOrEmpty(x.ResponsiblePerson));

            RuleFor(x => x.SupplierCustomer)
                .MaximumLength(100).WithMessage(Error.SupplierCustomerMaxLength)
                .When(x => !string.IsNullOrEmpty(x.SupplierCustomer));

            RuleFor(x => x.BatchNumber)
                .MaximumLength(50).WithMessage(Error.BatchNumberMaxLength)
                .When(x => !string.IsNullOrEmpty(x.BatchNumber));

            RuleFor(x => x.SerialNumber)
                .MaximumLength(50).WithMessage(Error.SerialNumberMaxLength)
                .When(x => !string.IsNullOrEmpty(x.SerialNumber));

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.Now).WithMessage(Error.ExpiryDatePast)
                .When(x => x.ExpiryDate.HasValue);

            RuleFor(x => x.Status)
                .MaximumLength(50).WithMessage(Error.StatusMaxLength)
                .Must(BeValidStatus).WithMessage(Error.StatusInvalid)
                .When(x => !string.IsNullOrEmpty(x.Status));
        }

        private static bool BeValidMovementType(string? movementType)
        {
            if (string.IsNullOrEmpty(movementType)) return false;
            
            var validTypes = new[] { "IN", "OUT", "TRANSFER", "ADJUSTMENT", "PRODUCTION", "CONSUMPTION", "RETURN" };
            return validTypes.Contains(movementType.ToUpper());
        }

        private static bool BeValidStatus(string? status)
        {
            if (string.IsNullOrEmpty(status)) return false;
            
            var validStatuses = new[] { "PENDING", "CONFIRMED", "CANCELLED", "COMPLETED" };
            return validStatuses.Contains(status.ToUpper());
        }
    }
} 