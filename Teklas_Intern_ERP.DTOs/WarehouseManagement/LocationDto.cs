using FluentValidation;

namespace Teklas_Intern_ERP.DTOs.WarehouseManagement
{
    public sealed class LocationDto
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public long? WarehouseId { get; set; }
        public string? LocationType { get; set; }
        public string? Aisle { get; set; }
        public string? Rack { get; set; }
        public string? Level { get; set; }
        public string? Position { get; set; }
        public decimal? Capacity { get; set; }
        public decimal? OccupiedCapacity { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public decimal? WeightCapacity { get; set; }
        public decimal? Temperature { get; set; }
        public decimal? Humidity { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        // Navigation properties for display
        public string? WarehouseName { get; set; }
        public string? WarehouseCode { get; set; }
    }

    public sealed class LocationDtoValidator : AbstractValidator<LocationDto>
    {
        public LocationDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("Location code is required")
                .MinimumLength(2).WithMessage("Location code must be at least 2 characters")
                .MaximumLength(20).WithMessage("Location code cannot exceed 20 characters");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Location name is required")
                .MinimumLength(2).WithMessage("Location name must be at least 2 characters")
                .MaximumLength(200).WithMessage("Location name cannot exceed 200 characters");

            RuleFor(x => x.WarehouseId)
                .NotNull().WithMessage("Warehouse is required")
                .GreaterThan(0).WithMessage("Warehouse is required");

            RuleFor(x => x.LocationType)
                .NotEmpty().WithMessage("Location type is required")
                .MaximumLength(50).WithMessage("Location type cannot exceed 50 characters");

            RuleFor(x => x.Aisle)
                .MaximumLength(10).WithMessage("Aisle cannot exceed 10 characters")
                .When(x => !string.IsNullOrEmpty(x.Aisle));

            RuleFor(x => x.Rack)
                .MaximumLength(10).WithMessage("Rack cannot exceed 10 characters")
                .When(x => !string.IsNullOrEmpty(x.Rack));

            RuleFor(x => x.Level)
                .MaximumLength(10).WithMessage("Level cannot exceed 10 characters")
                .When(x => !string.IsNullOrEmpty(x.Level));

            RuleFor(x => x.Position)
                .MaximumLength(10).WithMessage("Position cannot exceed 10 characters")
                .When(x => !string.IsNullOrEmpty(x.Position));

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0).When(x => x.Capacity.HasValue).WithMessage("Capacity must be positive");

            RuleFor(x => x.OccupiedCapacity)
                .GreaterThanOrEqualTo(0).When(x => x.OccupiedCapacity.HasValue).WithMessage("Occupied capacity must be positive");

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0).When(x => x.Length.HasValue).WithMessage("Length must be positive");

            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0).When(x => x.Width.HasValue).WithMessage("Width must be positive");

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0).When(x => x.Height.HasValue).WithMessage("Height must be positive");

            RuleFor(x => x.WeightCapacity)
                .GreaterThanOrEqualTo(0).When(x => x.WeightCapacity.HasValue).WithMessage("Weight capacity must be positive");

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