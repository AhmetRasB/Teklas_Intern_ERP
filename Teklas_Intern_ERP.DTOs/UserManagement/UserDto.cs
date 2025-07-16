using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    public sealed class UserDto
    {
       public long Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? ProfilePicture { get; set; }
    public DateTime? LastLoginDate { get; set; }
    public bool IsEmailConfirmed { get; set; }
    public int Status { get; set; }
    public DateTime CreateDate { get; set; }
    
    // Navigation Properties
    public List<string> RoleNames { get; set; } = new();
    
    // Computed Properties
    public string? FullName { get; set; }
    }

    public sealed class UserDtoValidator : AbstractValidator<UserDto>
    {
        public UserDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Error.UsernameRequired)
            .MinimumLength(3).WithMessage(Error.UsernameMinLength)
            .MaximumLength(50).WithMessage(Error.UsernameMaxLength);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Error.EmailRequired)
            .EmailAddress().WithMessage(Error.EmailInvalid)
            .MaximumLength(100).WithMessage(Error.EmailMaxLength);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(Error.FirstNameRequired)
            .MaximumLength(50).WithMessage(Error.FirstNameMaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(Error.LastNameRequired)
            .MaximumLength(50).WithMessage(Error.LastNameMaxLength);
    }
    }
} 