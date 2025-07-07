using FluentValidation;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP;

namespace Teklas_Intern_ERP.Validators
{
    public class MaterialCardValidator : AbstractValidator<MaterialCardDto>
    {
        public MaterialCardValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Code).NotEmpty();
        }
    }
} 