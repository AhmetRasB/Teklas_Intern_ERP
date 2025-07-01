using FluentValidation;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class MaterialMovementValidator : AbstractValidator<MaterialMovement>
    {
        public MaterialMovementValidator()
        {
            RuleFor(x => x.MaterialId)
                .GreaterThan(0).WithMessage("Malzeme seçilmelidir.");

            RuleFor(x => x.MovementType)
                .NotEmpty().WithMessage("Hareket tipi boş olamaz.")
                .MaximumLength(20);

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalı.");

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.TotalPrice)
                .GreaterThanOrEqualTo(0);

            RuleFor(x => x.ReferenceDocumentNo)
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .MaximumLength(250);
        }
    }
} 