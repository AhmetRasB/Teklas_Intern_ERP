using FluentValidation;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class MaterialBatchValidator : AbstractValidator<MaterialBatch>
    {
        public MaterialBatchValidator()
        {
            RuleFor(x => x.MaterialId)
                .GreaterThan(0).WithMessage("Malzeme seçilmelidir.");

            RuleFor(x => x.BatchNo)
                .NotEmpty().WithMessage("Parti/Lot numarası boş olamaz.")
                .MaximumLength(30);

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalı.");
        }
    }
} 