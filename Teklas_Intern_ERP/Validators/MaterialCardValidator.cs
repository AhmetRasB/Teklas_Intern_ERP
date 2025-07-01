using FluentValidation;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class MaterialCardValidator : AbstractValidator<MaterialCard>
    {
        public MaterialCardValidator()
        {
            RuleFor(x => x.MaterialCode)
                .NotEmpty().WithMessage("Malzeme kodu boş olamaz.")
                .MaximumLength(30);

            RuleFor(x => x.MaterialName)
                .NotEmpty().WithMessage("Malzeme adı boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.MaterialType)
                .NotEmpty().WithMessage("Malzeme tipi boş olamaz.")
                .MaximumLength(50);

            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Kategori seçilmelidir.");

            RuleFor(x => x.UnitOfMeasure)
                .NotEmpty().WithMessage("Birim boş olamaz.")
                .MaximumLength(10);

            RuleFor(x => x.Barcode)
                .MaximumLength(50);

            RuleFor(x => x.Brand)
                .MaximumLength(50);

            RuleFor(x => x.Model)
                .MaximumLength(50);

            RuleFor(x => x.PurchasePrice)
                .GreaterThanOrEqualTo(0).WithMessage("Alış fiyatı negatif olamaz.");

            RuleFor(x => x.SalesPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Satış fiyatı negatif olamaz.");

            RuleFor(x => x.MinimumStockLevel)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.MaximumStockLevel)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ReorderLevel)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ShelfLife)
                .GreaterThanOrEqualTo(0).When(x => x.ShelfLife.HasValue);

            RuleFor(x => x.Weight)
                .GreaterThanOrEqualTo(0).When(x => x.Weight.HasValue);

            RuleFor(x => x.Volume)
                .GreaterThanOrEqualTo(0).When(x => x.Volume.HasValue);

            RuleFor(x => x.Length)
                .GreaterThanOrEqualTo(0).When(x => x.Length.HasValue);

            RuleFor(x => x.Width)
                .GreaterThanOrEqualTo(0).When(x => x.Width.HasValue);

            RuleFor(x => x.Height)
                .GreaterThanOrEqualTo(0).When(x => x.Height.HasValue);

            RuleFor(x => x.Color)
                .MaximumLength(30);

            RuleFor(x => x.OriginCountry)
                .MaximumLength(50);

            RuleFor(x => x.Manufacturer)
                .MaximumLength(100);
        }
    }
} 