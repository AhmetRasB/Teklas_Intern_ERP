using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    public sealed class RoleDto
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? DisplayName { get; set; }
    public string? Description { get; set; }
    public string? Permissions { get; set; } // JSON format for permissions
    public string? Color { get; set; }
    public bool IsSystemRole { get; set; }
    public int Priority { get; set; }
    public int Status { get; set; }
    public DateTime CreateDate { get; set; }
    
    // Navigation Properties
    public int UserCount { get; set; }
    public List<string> PermissionList { get; set; } = new();
}

    public sealed class RoleDtoValidator : AbstractValidator<RoleDto>
    {
        public RoleDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage(Error.RoleNameRequired)
            .MinimumLength(2).WithMessage(Error.RoleNameMinLength)
            .MaximumLength(50).WithMessage(Error.RoleNameMaxLength);

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage(Error.RoleDescriptionMaxLength)
            .When(x => !string.IsNullOrEmpty(x.Description));

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage(Error.RolePriorityPositive);
    }
    }
} 