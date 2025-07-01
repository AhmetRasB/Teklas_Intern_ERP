using FluentValidation;
using Teklas_Intern_ERP.Entities.WarehouseManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            RuleFor(x => x.WarehouseCode)
                .NotEmpty().WithMessage("Depo kodu boş olamaz.")
                .MaximumLength(20);

            RuleFor(x => x.WarehouseName)
                .NotEmpty().WithMessage("Depo adı boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres boş olamaz.")
                .MaximumLength(250);

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir boş olamaz.")
                .MaximumLength(50);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Ülke boş olamaz.")
                .MaximumLength(50);

            RuleFor(x => x.ResponsiblePerson)
                .NotEmpty().WithMessage("Sorumlu kişi boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.Phone)
                .MaximumLength(20);

            RuleFor(x => x.Email)
                .EmailAddress().When(x => !string.IsNullOrEmpty(x.Email)).WithMessage("Geçerli bir e-posta giriniz.");

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0);
        }
    }
} 