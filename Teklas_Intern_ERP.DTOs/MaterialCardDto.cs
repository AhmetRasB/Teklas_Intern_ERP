using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    public sealed class MaterialCardDto
    {
        public int Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? MaterialType { get; set; }
        public int? CategoryId { get; set; }
        public string? UnitOfMeasure { get; set; }
        public string? Barcode { get; set; }
        public string? Description { get; set; }
        public string? Brand { get; set; }
        public string? Model { get; set; }
        public decimal? PurchasePrice { get; set; }
        public decimal? SalesPrice { get; set; }
        public decimal? MinimumStockLevel { get; set; }
        public decimal? MaximumStockLevel { get; set; }
        public decimal? ReorderLevel { get; set; }
        public int? ShelfLife { get; set; }
        public decimal? Weight { get; set; }
        public decimal? Volume { get; set; }
        public decimal? Length { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string? Color { get; set; }
        public string? OriginCountry { get; set; }
        public string? Manufacturer { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public System.DateTime? CreatedDate { get; set; }
        public System.DateTime? UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public sealed class MaterialCardDtoValidator : AbstractValidator<MaterialCardDto>
        {
            public MaterialCardDtoValidator()
            {
                // Required fields
                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage(Error.MaterialNameRequired)
                    .MinimumLength(2)
                    .WithMessage(Error.MaterialNameMinLength)
                    .MaximumLength(100)
                    .WithMessage(Error.MaterialNameMaxLength);

                RuleFor(x => x.Code)
                    .NotEmpty()
                    .WithMessage(Error.MaterialCodeRequired)
                    .MinimumLength(3)
                    .WithMessage(Error.MaterialCodeMinLength)
                    .MaximumLength(20)
                    .WithMessage(Error.MaterialCodeMaxLength);

                RuleFor(x => x.MaterialType)
                    .NotEmpty()
                    .WithMessage(Error.MaterialTypeRequired);

                RuleFor(x => x.CategoryId)
                    .NotNull()
                    .WithMessage(Error.CategoryIdRequired);

                RuleFor(x => x.UnitOfMeasure)
                    .NotEmpty()
                    .WithMessage(Error.UnitOfMeasureRequired);

                // Optional fields with validation
                RuleFor(x => x.Barcode)
                    .MaximumLength(50)
                    .WithMessage(Error.BarcodeMaxLength)
                    .When(x => !string.IsNullOrEmpty(x.Barcode));

                RuleFor(x => x.Description)
                    .MaximumLength(500)
                    .WithMessage(Error.DescriptionMaxLength)
                    .When(x => !string.IsNullOrEmpty(x.Description));

                RuleFor(x => x.Brand)
                    .MaximumLength(50)
                    .WithMessage(Error.BrandMaxLength)
                    .When(x => !string.IsNullOrEmpty(x.Brand));

                RuleFor(x => x.Model)
                    .MaximumLength(50)
                    .WithMessage(Error.ModelMaxLength)
                    .When(x => !string.IsNullOrEmpty(x.Model));

                RuleFor(x => x.Color)
                    .MaximumLength(30)
                    .WithMessage(Error.ColorMaxLength)
                    .When(x => !string.IsNullOrEmpty(x.Color));

                RuleFor(x => x.OriginCountry)
                    .MaximumLength(50)
                    .WithMessage(Error.OriginCountryMaxLength)
                    .When(x => !string.IsNullOrEmpty(x.OriginCountry));

                RuleFor(x => x.Manufacturer)
                    .MaximumLength(100)
                    .WithMessage(Error.ManufacturerMaxLength)
                    .When(x => !string.IsNullOrEmpty(x.Manufacturer));

                // Numeric validations
                RuleFor(x => x.PurchasePrice)
                    .GreaterThan(0)
                    .WithMessage(Error.PurchasePricePositive)
                    .When(x => x.PurchasePrice.HasValue);

                RuleFor(x => x.SalesPrice)
                    .GreaterThan(0)
                    .WithMessage(Error.SalesPricePositive)
                    .When(x => x.SalesPrice.HasValue);

                RuleFor(x => x.MinimumStockLevel)
                    .GreaterThan(0)
                    .WithMessage(Error.MinimumStockLevelPositive)
                    .When(x => x.MinimumStockLevel.HasValue);

                RuleFor(x => x.MaximumStockLevel)
                    .GreaterThan(0)
                    .WithMessage(Error.MaximumStockLevelPositive)
                    .When(x => x.MaximumStockLevel.HasValue);

                RuleFor(x => x.ReorderLevel)
                    .GreaterThan(0)
                    .WithMessage(Error.ReorderLevelPositive)
                    .When(x => x.ReorderLevel.HasValue);

                RuleFor(x => x.ShelfLife)
                    .GreaterThan(0)
                    .WithMessage(Error.ShelfLifePositive)
                    .When(x => x.ShelfLife.HasValue);

                RuleFor(x => x.Weight)
                    .GreaterThan(0)
                    .WithMessage(Error.WeightPositive)
                    .When(x => x.Weight.HasValue);

                RuleFor(x => x.Volume)
                    .GreaterThan(0)
                    .WithMessage(Error.VolumePositive)
                    .When(x => x.Volume.HasValue);

                RuleFor(x => x.Length)
                    .GreaterThan(0)
                    .WithMessage(Error.LengthPositive)
                    .When(x => x.Length.HasValue);

                RuleFor(x => x.Width)
                    .GreaterThan(0)
                    .WithMessage(Error.WidthPositive)
                    .When(x => x.Width.HasValue);

                RuleFor(x => x.Height)
                    .GreaterThan(0)
                    .WithMessage(Error.HeightPositive)
                    .When(x => x.Height.HasValue);

                // Business logic validations
                RuleFor(x => x.MaximumStockLevel)
                    .GreaterThan(x => x.MinimumStockLevel)
                    .WithMessage("Maksimum stok seviyesi minimum stok seviyesinden büyük olmalıdır.")
                    .When(x => x.MaximumStockLevel.HasValue && x.MinimumStockLevel.HasValue);

                RuleFor(x => x.ReorderLevel)
                    .LessThanOrEqualTo(x => x.MaximumStockLevel)
                    .WithMessage("Yeniden sipariş seviyesi maksimum stok seviyesinden küçük veya eşit olmalıdır.")
                    .When(x => x.ReorderLevel.HasValue && x.MaximumStockLevel.HasValue);
            }
        }
    }
} 