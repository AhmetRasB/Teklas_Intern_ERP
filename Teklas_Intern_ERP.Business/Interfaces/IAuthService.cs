using System.Threading.Tasks;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Interfaces
{
    public interface IAuthService
    {
        // Authentication Operations
        Task<(bool Success, string Token, UserDto User)> LoginAsync(LoginDto loginDto);
        Task<(bool Success, string Message, UserDto User)> RegisterAsync(RegisterDto registerDto);
        Task<bool> LogoutAsync(long userId);
        
        // Token Operations
        Task<(bool Success, string Token)> RefreshTokenAsync(string refreshToken);
        Task<bool> RevokeTokenAsync(string token);
        Task<bool> ValidateTokenAsync(string token);
        
        // Password Operations
        Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword);
        Task<bool> ResetPasswordAsync(string email);
        Task<bool> ConfirmPasswordResetAsync(string token, string newPassword);
        
        // Email Confirmation
        Task<bool> SendEmailConfirmationAsync(long userId);
        Task<bool> ConfirmEmailAsync(string token);
        
        // User Session
        Task<UserDto?> GetCurrentUserAsync(long userId);
        Task<bool> IsUserActiveAsync(long userId);
    }
} 