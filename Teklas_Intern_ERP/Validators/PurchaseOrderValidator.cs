using FluentValidation;
using Teklas_Intern_ERP.Entities.PurchasingManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class PurchaseOrderValidator : AbstractValidator<PurchaseOrder>
    {
        public PurchaseOrderValidator()
        {
            RuleFor(x => x.PurchaseOrderNo)
                .NotEmpty().WithMessage("Sipariş numarası boş olamaz.")
                .MaximumLength(30);

            RuleFor(x => x.SupplierId)
                .GreaterThan(0).WithMessage("Tedarikçi seçilmelidir.");

            RuleFor(x => x.OrderDate)
                .NotEmpty().WithMessage("Sipariş tarihi boş olamaz.");

            RuleFor(x => x.DeliveryDate)
                .NotEmpty().WithMessage("Teslimat tarihi boş olamaz.")
                .GreaterThanOrEqualTo(x => x.OrderDate).WithMessage("Teslimat tarihi sipariş tarihinden önce olamaz.");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("Durum boş olamaz.")
                .MaximumLength(30);

            RuleFor(x => x.TotalAmount)
                .GreaterThanOrEqualTo(0).WithMessage("Toplam tutar negatif olamaz.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Para birimi boş olamaz.")
                .MaximumLength(10);

            RuleFor(x => x.PaymentTerms)
                .NotEmpty().WithMessage("Ödeme şartları boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.DeliveryAddress)
                .NotEmpty().WithMessage("Teslimat adresi boş olamaz.")
                .MaximumLength(250);
        }
    }
} 