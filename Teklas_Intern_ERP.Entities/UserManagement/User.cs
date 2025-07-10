using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Teklas_Intern_ERP.Entities.UserManagement
{
    [Table("Users")]
    public class User : AuditEntity
    {
        [Required]
        [Column("Username")]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;
        
        [Required]
        [Column("Email")]
        [MaxLength(100)]
        public string Email { get; set; } = string.Empty;
        
        [Required]
        [Column("PasswordHash")]
        [MaxLength(255)]
        public string PasswordHash { get; set; } = string.Empty;
        
        [Required]
        [Column("PasswordSalt")]
        [MaxLength(255)]
        public string PasswordSalt { get; set; } = string.Empty;
        
        [Required]
        [Column("FirstName")]
        [MaxLength(50)]
        public string FirstName { get; set; } = string.Empty;
        
        [Required]
        [Column("LastName")]
        [MaxLength(50)]
        public string LastName { get; set; } = string.Empty;
        
        [Column("ProfilePicture")]
        [MaxLength(500)]
        public string? ProfilePicture { get; set; }
        
        [Column("PhoneNumber")]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }
        
        [Column("LastLoginDate")]
        public DateTime? LastLoginDate { get; set; }
        
        [Column("IsEmailConfirmed")]
        public bool IsEmailConfirmed { get; set; } = false;
        
        [Column("EmailConfirmationToken")]
        [MaxLength(255)]
        public string? EmailConfirmationToken { get; set; }
        
        [Column("PasswordResetToken")]
        [MaxLength(255)]
        public string? PasswordResetToken { get; set; }
        
        [Column("PasswordResetTokenExpiry")]
        public DateTime? PasswordResetTokenExpiry { get; set; }
        
        [Column("RefreshToken")]
        [MaxLength(255)]
        public string? RefreshToken { get; set; }
        
        [Column("RefreshTokenExpiry")]
        public DateTime? RefreshTokenExpiry { get; set; }
        
        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
        // Computed Properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}".Trim();
        
        /// <summary>
        /// User active status
        /// </summary>
        public new bool IsActive { get; set; } = true;
    }
}