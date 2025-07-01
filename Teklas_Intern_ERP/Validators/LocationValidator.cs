using FluentValidation;
using Teklas_Intern_ERP.Entities.WarehouseManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class LocationValidator : AbstractValidator<Location>
    {
        public LocationValidator()
        {
            RuleFor(x => x.WarehouseId)
                .GreaterThan(0).WithMessage("Depo seçilmelidir.");

            RuleFor(x => x.LocationCode)
                .NotEmpty().WithMessage("Lokasyon kodu boş olamaz.")
                .MaximumLength(20);

            RuleFor(x => x.LocationName)
                .NotEmpty().WithMessage("Lokasyon adı boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(250);

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0);
        }
    }
} 