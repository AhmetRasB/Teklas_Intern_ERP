using FluentValidation;

namespace Teklas_Intern_ERP.DTOs.PurchasingManagement
{
    public sealed class PurchaseOrderDto
    {
        public long Id { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public DateTime? ExpectedDeliveryDate { get; set; }
        public DateTime? ActualDeliveryDate { get; set; }
        public long? SupplierId { get; set; }
        public long? WarehouseId { get; set; }
        public string? OrderStatus { get; set; }
        public string? OrderType { get; set; }
        public int? PaymentTerms { get; set; }
        public string? Currency { get; set; }
        public decimal? ExchangeRate { get; set; }
        public decimal? Subtotal { get; set; }
        public decimal? TaxAmount { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? ShippingAmount { get; set; }
        public decimal? TotalAmount { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? DeliveryAddress { get; set; }
        public string? DeliveryCity { get; set; }
        public string? DeliveryCountry { get; set; }
        public string? DeliveryPostalCode { get; set; }
        public string? ShippingMethod { get; set; }
        public string? SpecialInstructions { get; set; }
        public string? Notes { get; set; }
        public long? ApprovedByUserId { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        // Navigation properties
        public string? SupplierName { get; set; }
        public string? SupplierCode { get; set; }
        public string? WarehouseName { get; set; }
    }

    public sealed class PurchaseOrderDtoValidator : AbstractValidator<PurchaseOrderDto>
    {
        public PurchaseOrderDtoValidator()
        {
            RuleFor(x => x.OrderNumber)
                .NotEmpty().WithMessage("Order number is required")
                .MinimumLength(2).WithMessage("Order number must be at least 2 characters")
                .MaximumLength(20).WithMessage("Order number cannot exceed 20 characters");

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Order date is required")
                .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order date cannot be in the future");

            RuleFor(x => x.ExpectedDeliveryDate)
                .GreaterThan(DateTime.UtcNow).When(x => x.ExpectedDeliveryDate.HasValue).WithMessage("Expected delivery date must be in the future");

            RuleFor(x => x.ActualDeliveryDate)
                .LessThanOrEqualTo(DateTime.UtcNow).When(x => x.ActualDeliveryDate.HasValue).WithMessage("Actual delivery date cannot be in the future");

            RuleFor(x => x.SupplierId)
                .NotNull().WithMessage("Supplier is required")
                .GreaterThan(0).WithMessage("Supplier is required");

            RuleFor(x => x.WarehouseId)
                .NotNull().WithMessage("Warehouse is required")
                .GreaterThan(0).WithMessage("Warehouse is required");

            RuleFor(x => x.OrderStatus)
                .NotEmpty().WithMessage("Order status is required")
                .MaximumLength(50).WithMessage("Order status cannot exceed 50 characters");

            RuleFor(x => x.OrderType)
                .MaximumLength(50).WithMessage("Order type cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.OrderType));

            RuleFor(x => x.PaymentTerms)
                .GreaterThanOrEqualTo(0).When(x => x.PaymentTerms.HasValue).WithMessage("Payment terms must be positive");

            RuleFor(x => x.Currency)
                .MaximumLength(3).WithMessage("Currency cannot exceed 3 characters")
                .When(x => !string.IsNullOrEmpty(x.Currency));

            RuleFor(x => x.ExchangeRate)
                .GreaterThan(0).When(x => x.ExchangeRate.HasValue).WithMessage("Exchange rate must be positive");

            RuleFor(x => x.Subtotal)
                .GreaterThanOrEqualTo(0).When(x => x.Subtotal.HasValue).WithMessage("Subtotal must be positive");

            RuleFor(x => x.TaxAmount)
                .GreaterThanOrEqualTo(0).When(x => x.TaxAmount.HasValue).WithMessage("Tax amount must be positive");

            RuleFor(x => x.DiscountAmount)
                .GreaterThanOrEqualTo(0).When(x => x.DiscountAmount.HasValue).WithMessage("Discount amount must be positive");

            RuleFor(x => x.ShippingAmount)
                .GreaterThanOrEqualTo(0).When(x => x.ShippingAmount.HasValue).WithMessage("Shipping amount must be positive");

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).When(x => x.TotalAmount.HasValue).WithMessage("Total amount must be positive");

            RuleFor(x => x.ReferenceNumber)
                .MaximumLength(50).WithMessage("Reference number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.ReferenceNumber));

            RuleFor(x => x.DeliveryAddress)
                .MaximumLength(500).WithMessage("Delivery address cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.DeliveryAddress));

            RuleFor(x => x.DeliveryCity)
                .MaximumLength(100).WithMessage("Delivery city cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.DeliveryCity));

            RuleFor(x => x.DeliveryCountry)
                .MaximumLength(100).WithMessage("Delivery country cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.DeliveryCountry));

            RuleFor(x => x.DeliveryPostalCode)
                .MaximumLength(20).WithMessage("Delivery postal code cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.DeliveryPostalCode));

            RuleFor(x => x.ShippingMethod)
                .MaximumLength(100).WithMessage("Shipping method cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.ShippingMethod));

            RuleFor(x => x.SpecialInstructions)
                .MaximumLength(1000).WithMessage("Special instructions cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.SpecialInstructions));

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }
    }
} 