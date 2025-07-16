using FluentValidation;

namespace Teklas_Intern_ERP.DTOs.WarehouseManagement
{
    public sealed class WarehouseDto
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? WarehouseType { get; set; }
        public string? Address { get; set; }
        public string? City { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
        public string? ManagerName { get; set; }
        public string? ManagerPhone { get; set; }
        public string? ManagerEmail { get; set; }
        public decimal? Capacity { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
    }

    public sealed class WarehouseDtoValidator : AbstractValidator<WarehouseDto>
    {
        public WarehouseDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Warehouse code is required")
                .MinimumLength(2).WithMessage("Warehouse code must be at least 2 characters")
                .MaximumLength(20).WithMessage("Warehouse code cannot exceed 20 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Warehouse name is required")
                .MinimumLength(2).WithMessage("Warehouse name must be at least 2 characters")
                .MaximumLength(200).WithMessage("Warehouse name cannot exceed 200 characters");

            RuleFor(x => x.WarehouseType)
                .NotEmpty().WithMessage("Warehouse type is required")
                .MaximumLength(50).WithMessage("Warehouse type cannot exceed 50 characters");

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

            RuleFor(x => x.ManagerName)
                .MaximumLength(100).WithMessage("Manager name cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.ManagerName));

            RuleFor(x => x.ManagerPhone)
                .MaximumLength(20).WithMessage("Manager phone cannot exceed 20 characters")
                .When(x => !string.IsNullOrEmpty(x.ManagerPhone));

            RuleFor(x => x.ManagerEmail)
                .EmailAddress().WithMessage("Invalid manager email format")
                .MaximumLength(100).WithMessage("Manager email cannot exceed 100 characters")
                .When(x => !string.IsNullOrEmpty(x.ManagerEmail));

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0).When(x => x.Capacity.HasValue).WithMessage("Capacity must be positive");

            RuleFor(x => x.Temperature)
                .InclusiveBetween(-50, 100).When(x => x.Temperature.HasValue).WithMessage("Temperature must be between -50 and 100");

            RuleFor(x => x.Humidity)
                .InclusiveBetween(0, 100).When(x => x.Humidity.HasValue).WithMessage("Humidity must be between 0 and 100");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
} 