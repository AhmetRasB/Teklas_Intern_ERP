using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DTOs;
using System;
using System.Threading.Tasks;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using ValidationException = FluentValidation.ValidationException;

namespace Teklas_Intern_ERP.Controllers.UserManagement
{
   
    [ApiController]
    [Route("api/auth")]
    [ApiExplorerSettings(GroupName = "User Management")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

       
        [HttpPost("login")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            try
            {
                var result = await _authService.LoginAsync(loginDto);
                
                if (!result.Success)
                {
                    return Unauthorized(new { error = "Kullanıcı adı veya şifre hatalı." });
                }

                return Ok(new
                {
                    success = true,
                    message = "Giriş başarılı.",
                    token = result.Token,
                    user = result.User
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// User registration
        /// </summary>
        /// <param name="registerDto">Registration information</param>
        /// <returns>Registration result and user information</returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(object), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                var result = await _authService.RegisterAsync(registerDto);
                
                if (!result.Success)
                {
                    return BadRequest(new { error = result.Message });
                }

                return StatusCode(201, new
                {
                    success = true,
                    message = result.Message,
                    user = result.User
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new { error = "Validation failed", details = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = "Business rule violation", details = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// User logout
        /// </summary>
        /// <returns>Logout result</returns>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                // Get user ID from claims
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { error = "Geçersiz token." });
                }

                var result = await _authService.LogoutAsync(userId);
                
                if (result)
                {
                    return Ok(new { success = true, message = "Çıkış başarılı." });
                }

                return BadRequest(new { error = "Çıkış işlemi başarısız." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Refresh JWT token
        /// </summary>
        /// <param name="refreshTokenDto">Refresh token</param>
        /// <returns>New JWT token</returns>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenDto refreshTokenDto)
        {
            try
            {
                var result = await _authService.RefreshTokenAsync(refreshTokenDto.RefreshToken!);
                
                if (!result.Success)
                {
                    return BadRequest(new { error = "Geçersiz refresh token." });
                }

                return Ok(new
                {
                    success = true,
                    message = "Token yenilendi.",
                    token = result.Token
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Change user password
        /// </summary>
        /// <param name="changePasswordDto">Current and new password</param>
        /// <returns>Password change result</returns>
        [HttpPost("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            try
            {
                // Get user ID from claims
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { error = "Geçersiz token." });
                }

                var result = await _authService.ChangePasswordAsync(userId, changePasswordDto.CurrentPassword!, changePasswordDto.NewPassword!);
                
                if (result)
                {
                    return Ok(new { success = true, message = "Şifre başarıyla değiştirildi." });
                }

                return BadRequest(new { error = "Mevcut şifre hatalı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Reset password request
        /// </summary>
        /// <param name="resetPasswordDto">Email address</param>
        /// <returns>Reset password result</returns>
        [HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
        {
            try
            {
                var result = await _authService.ResetPasswordAsync(resetPasswordDto.Email!);
                
                if (result)
                {
                    return Ok(new { success = true, message = "Şifre sıfırlama bağlantısı e-posta adresinize gönderildi." });
                }

                return BadRequest(new { error = "E-posta adresi bulunamadı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Confirm password reset
        /// </summary>
        /// <param name="confirmResetDto">Reset token and new password</param>
        /// <returns>Password reset confirmation result</returns>
        [HttpPost("confirm-reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordDto confirmResetDto)
        {
            try
            {
                var result = await _authService.ConfirmPasswordResetAsync(confirmResetDto.Token!, confirmResetDto.NewPassword!);
                
                if (result)
                {
                    return Ok(new { success = true, message = "Şifre başarıyla sıfırlandı." });
                }

                return BadRequest(new { error = "Geçersiz veya süresi dolmuş token." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Send email confirmation
        /// </summary>
        /// <returns>Email confirmation result</returns>
        [HttpPost("send-email-confirmation")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> SendEmailConfirmation()
        {
            try
            {
                // Get user ID from claims
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { error = "Geçersiz token." });
                }

                var result = await _authService.SendEmailConfirmationAsync(userId);
                
                if (result)
                {
                    return Ok(new { success = true, message = "E-posta doğrulama bağlantısı gönderildi." });
                }

                return BadRequest(new { error = "E-posta gönderimi başarısız." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Confirm email address
        /// </summary>
        /// <param name="confirmEmailDto">Email confirmation token</param>
        /// <returns>Email confirmation result</returns>
        [HttpPost("confirm-email")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmailDto confirmEmailDto)
        {
            try
            {
                var result = await _authService.ConfirmEmailAsync(confirmEmailDto.Token!);
                
                if (result)
                {
                    return Ok(new { success = true, message = "E-posta adresi başarıyla doğrulandı." });
                }

                return BadRequest(new { error = "Geçersiz doğrulama token'ı." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Get current user information
        /// </summary>
        /// <returns>Current user details</returns>
        [HttpGet("me")]
        [Authorize]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCurrentUser()
        {
            try
            {
                // Get user ID from claims
                var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
                if (!long.TryParse(userIdClaim, out var userId))
                {
                    return Unauthorized(new { error = "Geçersiz token." });
                }

                var user = await _authService.GetCurrentUserAsync(userId);
                
                if (user == null)
                {
                    return NotFound(new { error = "Kullanıcı bulunamadı." });
                }

                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }
    }

    #region Additional DTOs for Auth Endpoints

    public sealed class RefreshTokenDto
    {
        public string? RefreshToken { get; set; }
    }

    public sealed class ChangePasswordDto
    {
        public string? CurrentPassword { get; set; }
        public string? NewPassword { get; set; }
    }

    public sealed class ResetPasswordDto
    {
        public string? Email { get; set; }
    }

    public sealed class ConfirmResetPasswordDto
    {
        public string? Token { get; set; }
        public string? NewPassword { get; set; }
    }

    public sealed class ConfirmEmailDto
    {
        public string? Token { get; set; }
    }

    #endregion
}
