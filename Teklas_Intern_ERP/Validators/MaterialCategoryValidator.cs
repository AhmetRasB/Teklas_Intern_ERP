using FluentValidation;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Validators
{
    public class MaterialCategoryValidator : AbstractValidator<MaterialCategory>
    {
        public MaterialCategoryValidator()
        {
            RuleFor(x => x.CategoryCode)
                .NotEmpty().WithMessage("Kategori kodu boş olamaz.")
                .MaximumLength(20);

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Kategori adı boş olamaz.")
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(250);
        }
    }
} 