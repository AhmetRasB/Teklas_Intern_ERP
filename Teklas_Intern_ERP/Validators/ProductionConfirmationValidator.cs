using FluentValidation;
using Teklas_Intern_ERP.Entities.ProductionManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class ProductionConfirmationValidator : AbstractValidator<ProductionConfirmation>
    {
        public ProductionConfirmationValidator()
        {
            RuleFor(x => x.WorkOrderId)
                .GreaterThan(0).WithMessage("İş emri seçilmelidir.");

            RuleFor(x => x.ConfirmedQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Onaylanan miktar negatif olamaz.");

            RuleFor(x => x.ScrapQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Fire miktarı negatif olamaz.");

            RuleFor(x => x.ConfirmationDate)
                .NotEmpty().WithMessage("Onay tarihi boş olamaz.");

            RuleFor(x => x.ConfirmedBy)
                .NotEmpty().WithMessage("Onaylayan kişi boş olamaz.")
                .MaximumLength(100);
        }
    }
} 