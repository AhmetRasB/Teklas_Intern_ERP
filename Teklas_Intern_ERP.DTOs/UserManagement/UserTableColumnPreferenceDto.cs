using FluentValidation;
using System;

namespace Teklas_Intern_ERP.DTOs.UserManagement
{
    public sealed class UserTableColumnPreferenceDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string TableKey { get; set; } = string.Empty;
        public string ColumnsJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }

    public sealed class UserTableColumnPreferenceDtoValidator : AbstractValidator<UserTableColumnPreferenceDto>
    {
        public UserTableColumnPreferenceDtoValidator()
        {
            RuleFor(x => x.UserId).GreaterThan(0);
            RuleFor(x => x.TableKey).NotEmpty().MaximumLength(100);
            RuleFor(x => x.ColumnsJson).NotEmpty();
        }
    }
} 