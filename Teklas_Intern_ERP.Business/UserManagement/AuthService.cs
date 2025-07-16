using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentValidation;
using Teklas_Intern_ERP.Business.Interfaces;
using Teklas_Intern_ERP.DataAccess.UserManagement;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.DTOs;
using Teklas_Intern_ERP.Entities.UserManagement;
using ValidationException = FluentValidation.ValidationException;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using Teklas_Intern_ERP.Business.Helpers;

namespace Teklas_Intern_ERP.Business.UserManagement
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly JwtHelper _jwtHelper;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            JwtHelper jwtHelper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
        }

        #region Authentication Operations

        public async Task<(bool Success, string Token, UserDto User)> LoginAsync(LoginDto loginDto)
        {
            // VALIDATION
            var validator = new LoginDtoValidator();
            var validationResult = validator.Validate(loginDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                // Find user by username or email
                var user = await _userRepository.GetByUsernameOrEmailAsync(loginDto.UsernameOrEmail!);
                if (user == null)
                    return (false, string.Empty, new UserDto());

                // Verify password
                if (!VerifyPassword(loginDto.Password!, user.PasswordHash))
                    return (false, string.Empty, new UserDto());

                // Check if user is active
                if (!user.IsActive)
                    return (false, string.Empty, new UserDto());

                // Update last login
                await _userRepository.UpdateLastLoginAsync(user.Id);

                // Get user with roles
                var userWithRoles = await _userRepository.GetUserWithRolesAsync(user.Id);
                var userDto = _mapper.Map<UserDto>(userWithRoles);
                
                // Get user roles and permissions
                var userRoles = await _roleRepository.GetUserRolesAsync(user.Id);
                var roleNames = userRoles.Select(r => r.Name).ToList();
                var permissions = await _roleRepository.GetUserPermissionsAsync(user.Id);
                
                // Generate JWT token
                var token = _jwtHelper.GenerateToken(userDto, roleNames, permissions);

                return (true, token, userDto);
            }
            catch
            {
                return (false, string.Empty, new UserDto());
            }
        }

        public async Task<(bool Success, string Message, UserDto User)> RegisterAsync(RegisterDto registerDto)
        {
            // VALIDATION
            var validator = new RegisterDtoValidator();
            var validationResult = validator.Validate(registerDto);
            if (!validationResult.IsValid)
                throw new ValidationException(validationResult.Errors);

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                // Check if username is unique
                if (!await _userRepository.IsUsernameUniqueAsync(registerDto.Username!))
                    return (false, "Bu kullanıcı adı zaten kullanılmaktadır.", new UserDto());

                // Check if email is unique
                if (!await _userRepository.IsEmailUniqueAsync(registerDto.Email!))
                    return (false, "Bu e-posta adresi zaten kullanılmaktadır.", new UserDto());

                // Create user entity
                var passwordSalt = GenerateRandomToken();
                var user = new User
                {
                    Username = registerDto.Username!,
                    Email = registerDto.Email!,
                    PasswordHash = HashPassword(registerDto.Password!),
                    PasswordSalt = passwordSalt,
                    FirstName = registerDto.FirstName!,
                    LastName = registerDto.LastName!,
                    PhoneNumber = registerDto.PhoneNumber,
                    Status = Entities.StatusType.Active,
                    CreateUserId = 1, // TODO: Get from current user context
                    IsEmailConfirmed = false,
                    EmailConfirmationToken = GenerateRandomToken()
                };

                var addedUser = await _userRepository.AddAsync(user);

                // Assign default role (User) - Create if doesn't exist
                var defaultRole = await _roleRepository.GetByNameAsync("User");
                if (defaultRole == null)
                {
                    defaultRole = new Role
                    {
                        Name = "User",
                        DisplayName = "Kullanıcı",
                        Description = "Standart kullanıcı rolü",
                        IsSystemRole = true,
                        Priority = 100,
                        Status = Entities.StatusType.Active,
                        CreateUserId = 1
                    };
                    defaultRole = await _roleRepository.AddAsync(defaultRole);
                }
                
                await _roleRepository.AddUserToRoleAsync(addedUser.Id, defaultRole.Id);

                await _unitOfWork.CommitTransactionAsync();

                var userDto = _mapper.Map<UserDto>(addedUser);
                return (true, "Kayıt başarılı. E-posta adresinizi doğrulayın.", userDto);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync();
                // Log the actual exception for debugging
                Console.WriteLine($"Register Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                return (false, $"Kayıt sırasında bir hata oluştu: {ex.Message}", new UserDto());
            }
        }

        public async Task<bool> LogoutAsync(long userId)
        {
            // TODO: Implement token revocation/blacklist
            await Task.CompletedTask;
            return true;
        }

        #endregion

        #region Token Operations

        public async Task<(bool Success, string Token)> RefreshTokenAsync(string refreshToken)
        {
            try
            {
                // TODO: Implement refresh token storage and validation
                // For now, just generate a new refresh token
                var newRefreshToken = _jwtHelper.GenerateRefreshToken();
                await Task.CompletedTask;
                return (true, newRefreshToken);
            }
            catch
            {
                return (false, string.Empty);
            }
        }

        public async Task<bool> RevokeTokenAsync(string token)
        {
            // TODO: Implement token revocation
            await Task.CompletedTask;
            return true;
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            await Task.CompletedTask;
            return _jwtHelper.ValidateToken(token) != null;
        }

        #endregion

        #region Password Operations

        public async Task<bool> ChangePasswordAsync(long userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                if (!VerifyPassword(currentPassword, user.PasswordHash))
                    return false;

                var newPasswordHash = HashPassword(newPassword);
                await _userRepository.UpdatePasswordAsync(userId, newPasswordHash);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ResetPasswordAsync(string email)
        {
            try
            {
                var user = await _userRepository.GetByEmailAsync(email);
                if (user == null) return false;

                var resetToken = GenerateRandomToken();
                user.PasswordResetToken = resetToken;
                user.PasswordResetTokenExpiry = DateTime.UtcNow.AddHours(24);

                await _userRepository.UpdateAsync(user);

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ConfirmPasswordResetAsync(string token, string newPassword)
        {
            try
            {
                var users = await _userRepository.FindAsync(u => u.PasswordResetToken == token);
                var user = users.FirstOrDefault();

                if (user == null || user.PasswordResetTokenExpiry < DateTime.UtcNow)
                    return false;

                // Update password and clear reset token
                user.PasswordHash = HashPassword(newPassword);
                user.PasswordResetToken = null;
                user.PasswordResetTokenExpiry = null;

                await _userRepository.UpdateAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Email Confirmation

        public async Task<bool> SendEmailConfirmationAsync(long userId)
        {
            try
            {
                var user = await _userRepository.GetByIdAsync(userId);
                if (user == null) return false;

                user.EmailConfirmationToken = GenerateRandomToken();
                await _userRepository.UpdateAsync(user);

                // TODO: Send confirmation email
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ConfirmEmailAsync(string token)
        {
            try
            {
                var users = await _userRepository.FindAsync(u => u.EmailConfirmationToken == token);
                var user = users.FirstOrDefault();

                if (user == null) return false;

                user.IsEmailConfirmed = true;
                user.EmailConfirmationToken = null;

                await _userRepository.UpdateAsync(user);
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region User Session

        public async Task<UserDto?> GetCurrentUserAsync(long userId)
        {
            var user = await _userRepository.GetUserWithRolesAsync(userId);
            return user != null ? _mapper.Map<UserDto>(user) : null;
        }

        public async Task<bool> IsUserActiveAsync(long userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user?.IsActive ?? false;
        }

        #endregion

        #region Private Helper Methods

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "TeklasSalt"));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string hash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == hash;
        }

        private string GenerateRandomToken()
        {
            var randomBytes = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }



        #endregion
    }
}
