using FluentValidation;
using Teklas_Intern_ERP.Entities.ProductionManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class BillOfMaterialsValidator : AbstractValidator<BillOfMaterials>
    {
        public BillOfMaterialsValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Ürün seçilmelidir.");

            RuleFor(x => x.MaterialId)
                .GreaterThan(0).WithMessage("Malzeme seçilmelidir.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalı.");

            RuleFor(x => x.UnitOfMeasure)
                .NotEmpty().WithMessage("Birim boş olamaz.")
                .MaximumLength(10);

            RuleFor(x => x.ScrapRate)
                .GreaterThanOrEqualTo(0);
        }
    }
} 