using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    public sealed class MaterialCardDto
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? MaterialType { get; set; }
        public long? CategoryId { get; set; }
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
        public string? ManufacturerPartNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        
        // Navigation property
        public string? CategoryName { get; set; }
    }

    public sealed class MaterialCardDtoValidator : AbstractValidator<MaterialCardDto>
    {
        public MaterialCardDtoValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage(Error.MaterialCodeRequired)
                .MinimumLength(3).WithMessage(Error.MaterialCodeMinLength)
                .MaximumLength(20).WithMessage(Error.MaterialCodeMaxLength);

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(Error.MaterialNameRequired)
                .MinimumLength(2).WithMessage(Error.MaterialNameMinLength)
                .MaximumLength(100).WithMessage(Error.MaterialNameMaxLength);

            RuleFor(x => x.MaterialType)
                .NotEmpty().WithMessage(Error.MaterialTypeRequired);

            RuleFor(x => x.CategoryId)
                .NotNull().WithMessage(Error.CategoryIdRequired)
                .GreaterThan(0).WithMessage(Error.CategoryIdRequired);

            RuleFor(x => x.UnitOfMeasure)
                .NotEmpty().WithMessage(Error.UnitOfMeasureRequired);

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).When(x => x.PurchasePrice.HasValue).WithMessage(Error.PurchasePricePositive);

            RuleFor(x => x.SalesPrice)
                .GreaterThanOrEqualTo(0).When(x => x.SalesPrice.HasValue).WithMessage(Error.SalesPricePositive);

            RuleFor(x => x.MinimumStockLevel)
                .GreaterThanOrEqualTo(0).When(x => x.MinimumStockLevel.HasValue).WithMessage(Error.MinimumStockLevelPositive);

            RuleFor(x => x.MaximumStockLevel)
                .GreaterThanOrEqualTo(0).When(x => x.MaximumStockLevel.HasValue).WithMessage(Error.MaximumStockLevelPositive);

            RuleFor(x => x.ReorderLevel)
                .GreaterThanOrEqualTo(0).When(x => x.ReorderLevel.HasValue).WithMessage(Error.ReorderLevelPositive);

            RuleFor(x => x.ShelfLife)
                .GreaterThanOrEqualTo(0).When(x => x.ShelfLife.HasValue).WithMessage(Error.ShelfLifePositive);

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).When(x => x.Weight.HasValue).WithMessage(Error.WeightPositive);

            RuleFor(x => x.Volume)
                .GreaterThanOrEqualTo(0).When(x => x.Volume.HasValue).WithMessage(Error.VolumePositive);

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0).When(x => x.Length.HasValue).WithMessage(Error.LengthPositive);

            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0).When(x => x.Width.HasValue).WithMessage(Error.WidthPositive);

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0).When(x => x.Height.HasValue).WithMessage(Error.HeightPositive);

            RuleFor(x => x.Barcode)
                .MaximumLength(50).WithMessage(Error.BarcodeMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Barcode));

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage(Error.DescriptionMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Description));

            RuleFor(x => x.Brand)
                .MaximumLength(50).WithMessage(Error.BrandMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Brand));

            RuleFor(x => x.Model)
                .MaximumLength(50).WithMessage(Error.ModelMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Model));

            RuleFor(x => x.Color)
                .MaximumLength(30).WithMessage(Error.ColorMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Color));

            RuleFor(x => x.OriginCountry)
                .MaximumLength(50).WithMessage(Error.OriginCountryMaxLength)
                .When(x => !string.IsNullOrEmpty(x.OriginCountry));

            RuleFor(x => x.Manufacturer)
                .MaximumLength(100).WithMessage(Error.ManufacturerMaxLength)
                .When(x => !string.IsNullOrEmpty(x.Manufacturer));
        }
    }
} 