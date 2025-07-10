using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Teklas_Intern_ERP.DTOs;

namespace Teklas_Intern_ERP.Business.Helpers
{
    public class JwtHelper
    {
        private readonly IConfiguration _configuration;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly int _expiryInMinutes;

        public JwtHelper(IConfiguration configuration)
        {
            _configuration = configuration;
            _secretKey = _configuration["JwtSettings:SecretKey"]!;
            _issuer = _configuration["JwtSettings:Issuer"]!;
            _audience = _configuration["JwtSettings:Audience"]!;
            _expiryInMinutes = int.Parse(_configuration["JwtSettings:ExpiryInMinutes"]!);
        }

        #region Token Generation

        public string GenerateToken(UserDto user, List<string> roles, List<string> permissions)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.Username ?? string.Empty),
                new(ClaimTypes.Email, user.Email ?? string.Empty),
                new(ClaimTypes.GivenName, user.FirstName ?? string.Empty),
                new(ClaimTypes.Surname, user.LastName ?? string.Empty),
                new("FullName", user.FullName ?? string.Empty),
                new("IsEmailConfirmed", user.IsEmailConfirmed.ToString()),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            // Add roles as claims
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            // Add permissions as claims
            foreach (var permission in permissions)
            {
                claims.Add(new Claim("permission", permission));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(_expiryInMinutes),
                Issuer = _issuer,
                Audience = _audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateRefreshToken()
        {
            var randomBytes = new byte[64];
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        #endregion

        #region Token Validation

        public ClaimsPrincipal? ValidateToken(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_secretKey);

                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _issuer,
                    ValidateAudience = true,
                    ValidAudience = _audience,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                // Check if the token is a JWT token
                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        public bool IsTokenExpired(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                
                return jwtToken.ValidTo < DateTime.UtcNow;
            }
            catch
            {
                return true;
            }
        }

        #endregion

        #region Claims Helper Methods

        public long GetUserIdFromToken(string token)
        {
            var principal = ValidateToken(token);
            var userIdClaim = principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            return long.TryParse(userIdClaim, out var userId) ? userId : 0;
        }

        public string GetUsernameFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindFirst(ClaimTypes.Name)?.Value ?? string.Empty;
        }

        public string GetEmailFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindFirst(ClaimTypes.Email)?.Value ?? string.Empty;
        }

        public List<string> GetRolesFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindAll(ClaimTypes.Role)?.Select(c => c.Value).ToList() ?? new List<string>();
        }

        public List<string> GetPermissionsFromToken(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindAll("permission")?.Select(c => c.Value).ToList() ?? new List<string>();
        }

        public bool HasRole(string token, string role)
        {
            var roles = GetRolesFromToken(token);
            return roles.Contains(role, StringComparer.OrdinalIgnoreCase);
        }

        public bool HasPermission(string token, string permission)
        {
            var permissions = GetPermissionsFromToken(token);
            return permissions.Contains(permission, StringComparer.OrdinalIgnoreCase);
        }

        public bool HasAnyRole(string token, params string[] roles)
        {
            var userRoles = GetRolesFromToken(token);
            return roles.Any(role => userRoles.Contains(role, StringComparer.OrdinalIgnoreCase));
        }

        public bool HasAnyPermission(string token, params string[] permissions)
        {
            var userPermissions = GetPermissionsFromToken(token);
            return permissions.Any(permission => userPermissions.Contains(permission, StringComparer.OrdinalIgnoreCase));
        }

        #endregion

        #region Token Info

        public DateTime GetTokenExpiration(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                return jwtToken.ValidTo;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public DateTime GetTokenIssuedAt(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                return jwtToken.ValidFrom;
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        public string GetTokenId(string token)
        {
            var principal = ValidateToken(token);
            return principal?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value ?? string.Empty;
        }

        public TimeSpan GetRemainingTime(string token)
        {
            var expiration = GetTokenExpiration(token);
            var remaining = expiration - DateTime.UtcNow;
            return remaining > TimeSpan.Zero ? remaining : TimeSpan.Zero;
        }

        #endregion

        #region Static Helper Methods

        public static string ExtractTokenFromHeader(string authorizationHeader)
        {
            if (string.IsNullOrEmpty(authorizationHeader))
                return string.Empty;

            if (authorizationHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                return authorizationHeader.Substring("Bearer ".Length).Trim();

            return string.Empty;
        }

        public static bool IsValidTokenFormat(string token)
        {
            if (string.IsNullOrEmpty(token))
                return false;

            var parts = token.Split('.');
            return parts.Length == 3; // JWT has 3 parts: header.payload.signature
        }

        #endregion
    }
}
