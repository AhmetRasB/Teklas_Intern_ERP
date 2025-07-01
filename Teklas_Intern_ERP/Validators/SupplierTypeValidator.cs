using FluentValidation;
using Teklas_Intern_ERP.Entities.PurchasingManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class SupplierTypeValidator : AbstractValidator<SupplierType>
    {
        public SupplierTypeValidator()
        {
            RuleFor(x => x.TypeName)
                .NotEmpty().WithMessage("Tedarikçi tipi adı boş olamaz.")
                .MaximumLength(50);

            RuleFor(x => x.Description)
                .MaximumLength(250);
        }
    }
} 