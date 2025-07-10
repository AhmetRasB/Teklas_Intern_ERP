using FluentValidation;

namespace Teklas_Intern_ERP.DTOs
{
   public sealed class LoginDto
{
    public string? UsernameOrEmail { get; set; }
    public string? Password { get; set; }
    public bool RememberMe { get; set; } = false;
}

    public sealed class LoginDtoValidator : AbstractValidator<LoginDto>
    {
       public LoginDtoValidator()
    {
        RuleFor(x => x.UsernameOrEmail)
            .NotEmpty().WithMessage(Error.UsernameOrEmailRequired);

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage(Error.PasswordRequired);
    }
    }
} 