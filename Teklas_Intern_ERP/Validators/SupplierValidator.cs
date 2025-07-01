using FluentValidation;
using Teklas_Intern_ERP.Entities.PurchasingManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class SupplierValidator : AbstractValidator<Supplier>
    {
        public SupplierValidator()
        {
            RuleFor(x => x.SupplierCode)
                .NotEmpty().WithMessage("Tedarikçi kodu boş olamaz.")
                .MaximumLength(20);

            RuleFor(x => x.SupplierName)
                .NotEmpty().WithMessage("Tedarikçi adı boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.TaxNumber)
                .NotEmpty().WithMessage("Vergi numarası boş olamaz.")
                .Length(10, 11).WithMessage("Vergi numarası 10 veya 11 haneli olmalı.");

            RuleFor(x => x.TaxOffice)
                .NotEmpty().WithMessage("Vergi dairesi boş olamaz.")
                .MaximumLength(50);

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Adres boş olamaz.")
                .MaximumLength(250);

            RuleFor(x => x.City)
                .NotEmpty().WithMessage("Şehir boş olamaz.")
                .MaximumLength(50);

            RuleFor(x => x.Country)
                .NotEmpty().WithMessage("Ülke boş olamaz.")
                .MaximumLength(50);

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Telefon boş olamaz.")
                .Matches(@"^\+?[0-9]{10,15}$").WithMessage("Geçerli bir telefon numarası giriniz.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("E-posta boş olamaz.")
                .EmailAddress().WithMessage("Geçerli bir e-posta adresi giriniz.");

            RuleFor(x => x.ContactPerson)
                .NotEmpty().WithMessage("İrtibat kişisi boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.IBAN)
                .NotEmpty().WithMessage("IBAN boş olamaz.")
                .Length(26).WithMessage("IBAN 26 karakter olmalı.");

            RuleFor(x => x.BankName)
                .NotEmpty().WithMessage("Banka adı boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.SupplierTypeId)
                .GreaterThan(0).WithMessage("Tedarikçi tipi seçilmelidir.");
        }
    }
} 