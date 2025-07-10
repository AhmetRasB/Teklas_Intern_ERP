using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
    public sealed class RegisterDto
{
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public string? ConfirmPassword { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? PhoneNumber { get; set; }
}

    public sealed class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage(Error.UsernameRequired)
            .MinimumLength(3).WithMessage(Error.UsernameMinLength)
            .MaximumLength(50).WithMessage(Error.UsernameMaxLength)
            .Matches("^[a-zA-Z0-9_]+$").WithMessage(Error.UsernameInvalidChars);

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage(Error.EmailRequired)
            .EmailAddress().WithMessage(Error.EmailInvalid)
            .MaximumLength(100).WithMessage(Error.EmailMaxLength);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Error.PasswordRequired)
            .MinimumLength(6).WithMessage(Error.PasswordMinLength)
            .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)").WithMessage(Error.PasswordComplexity);

        RuleFor(x => x.ConfirmPassword)
            .NotEmpty().WithMessage(Error.ConfirmPasswordRequired)
            .Equal(x => x.Password).WithMessage(Error.PasswordsNotMatch);

        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage(Error.FirstNameRequired)
            .MaximumLength(50).WithMessage(Error.FirstNameMaxLength);

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage(Error.LastNameRequired)
            .MaximumLength(50).WithMessage(Error.LastNameMaxLength);
    }
    }
} 