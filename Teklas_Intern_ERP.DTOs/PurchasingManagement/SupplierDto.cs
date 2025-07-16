using FluentValidation;

namespace Teklas_Intern_ERP.DTOs.PurchasingManagement
{
    public sealed class SupplierDto
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? SupplierTypeId { get; set; }
        public string? ContactPerson { get; set; }
        public string? ContactPhone { get; set; }
        public string? ContactEmail { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? Website { get; set; }
        public string? TaxNumber { get; set; }
        public int? PaymentTerms { get; set; }
        public decimal? CreditLimit { get; set; }
        public decimal? CurrentBalance { get; set; }
        public string? Currency { get; set; }
        public string? BankAccount { get; set; }
        public string? BankName { get; set; }
        public string? IBAN { get; set; }
        public string? Notes { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        // Navigation property
        public string? SupplierTypeName { get; set; }
    }

    public sealed class SupplierDtoValidator : AbstractValidator<SupplierDto>
    {
        public SupplierDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Supplier code is required")
                .MinimumLength(2).WithMessage("Supplier code must be at least 2 characters")
                .MaximumLength(20).WithMessage("Supplier code cannot exceed 20 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Supplier name is required")
                .MinimumLength(2).WithMessage("Supplier name must be at least 2 characters")
                .MaximumLength(200).WithMessage("Supplier name cannot exceed 200 characters");

            RuleFor(x => x.SupplierTypeId)
                .NotNull().WithMessage("Supplier type is required")
                .GreaterThan(0).WithMessage("Supplier type is required");

            RuleFor(x => x.ContactPerson)
                .MaximumLength(100).WithMessage("Contact person cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.ContactPerson));

            RuleFor(x => x.ContactPhone)
                .MaximumLength(20).WithMessage("Contact phone cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.ContactPhone));

            RuleFor(x => x.ContactEmail)
                .EmailAddress().WithMessage("Invalid contact email format")
                .MaximumLength(100).WithMessage("Contact email cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.ContactEmail));

            RuleFor(x => x.Address)
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters")
                .When(x => !string.IsNullOrEmpty(x.Address));

            RuleFor(x => x.City)
                .MaximumLength(100).WithMessage("City cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.City));

            RuleFor(x => x.Country)
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Country));

            RuleFor(x => x.PostalCode)
                .MaximumLength(20).WithMessage("Postal code cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.PostalCode));

            RuleFor(x => x.Phone)
                .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.Phone));

            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("Invalid email format")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.Email));

            RuleFor(x => x.Website)
                .MaximumLength(200).WithMessage("Website cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.Website));

            RuleFor(x => x.TaxNumber)
                .MaximumLength(50).WithMessage("Tax number cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.TaxNumber));

            RuleFor(x => x.PaymentTerms)
                .GreaterThanOrEqualTo(0).When(x => x.PaymentTerms.HasValue).WithMessage("Payment terms must be positive");

            RuleFor(x => x.CreditLimit)
                .GreaterThanOrEqualTo(0).When(x => x.CreditLimit.HasValue).WithMessage("Credit limit must be positive");

            RuleFor(x => x.CurrentBalance)
                .GreaterThanOrEqualTo(0).When(x => x.CurrentBalance.HasValue).WithMessage("Current balance must be positive");

            RuleFor(x => x.Currency)
                .MaximumLength(3).WithMessage("Currency cannot exceed 3 characters")
                .When(x => !string.IsNullOrEmpty(x.Currency));

            RuleFor(x => x.BankAccount)
                .MaximumLength(200).WithMessage("Bank account cannot exceed 200 characters")
                .When(x => !string.IsNullOrEmpty(x.BankAccount));

            RuleFor(x => x.BankName)
                .MaximumLength(100).WithMessage("Bank name cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.BankName));

            RuleFor(x => x.IBAN)
                .MaximumLength(50).WithMessage("IBAN cannot exceed 50 characters")
                .When(x => !string.IsNullOrEmpty(x.IBAN));

            RuleFor(x => x.Notes)
                .MaximumLength(1000).WithMessage("Notes cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Notes));
        }
    }
} 