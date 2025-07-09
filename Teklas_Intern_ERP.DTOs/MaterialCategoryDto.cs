using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    public sealed class MaterialCategoryDto
    {
        public long Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public long? ParentCategoryId { get; set; }
        public string? ParentCategoryName { get; set; }
        public int Status { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long CreateUserId { get; set; }
        public long? UpdateUserId { get; set; }

        public sealed class MaterialCategoryDtoValidator : AbstractValidator<MaterialCategoryDto>
        {
            public MaterialCategoryDtoValidator()
            {
                // Required fields
                RuleFor(x => x.Code)
                    .NotEmpty()
                    .WithMessage(Error.CategoryCodeRequired)
                    .MaximumLength(50)
                    .WithMessage(Error.CategoryCodeMaxLength);

                RuleFor(x => x.Name)
                    .NotEmpty()
                    .WithMessage(Error.CategoryNameRequired)
                    .MaximumLength(100)
                    .WithMessage(Error.CategoryNameMaxLength);

                // Optional fields with validation
                RuleFor(x => x.Description)
                    .MaximumLength(500)
                    .WithMessage(Error.CategoryDescriptionMaxLength)
                    .When(x => !string.IsNullOrEmpty(x.Description));

                // Status validation
                RuleFor(x => x.Status)
                    .InclusiveBetween(0, 1)
                    .WithMessage("Durum 0 (Pasif) veya 1 (Aktif) olmalıdır.");

                // ParentCategoryId validation  
                RuleFor(x => x.ParentCategoryId)
                    .GreaterThan(0)
                    .WithMessage("Üst kategori ID'si pozitif olmalıdır.")
                    .When(x => x.ParentCategoryId.HasValue);
            }
        }
    }
} 