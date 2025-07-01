using FluentValidation;
using Teklas_Intern_ERP.Entities.WarehouseManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class StockEntryValidator : AbstractValidator<StockEntry>
    {
        public StockEntryValidator()
        {
            RuleFor(x => x.MaterialId)
                .GreaterThan(0).WithMessage("Malzeme seçilmelidir.");

            RuleFor(x => x.WarehouseId)
                .GreaterThan(0).WithMessage("Depo seçilmelidir.");

            RuleFor(x => x.LocationId)
                .GreaterThan(0).WithMessage("Lokasyon seçilmelidir.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Miktar sıfırdan büyük olmalı.");

            RuleFor(x => x.EntryDate)
                .NotEmpty().WithMessage("Giriş tarihi boş olamaz.");

            RuleFor(x => x.BatchNo)
                .MaximumLength(30);

            RuleFor(x => x.LotNo)
                .MaximumLength(30);

            RuleFor(x => x.UnitPrice)
                .GreaterThanOrEqualTo(0);
        }
    }
} 