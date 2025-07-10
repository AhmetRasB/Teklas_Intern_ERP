using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teklas_Intern_ERP.Entities.UserManagement
{
    [Table("UserRoles")]
    public class UserRole : AuditEntity
    {
        [Required]
        [ForeignKey("User")]
        public long UserId { get; set; }
        public virtual User User { get; set; } = null!;
        
        [Required]
        [ForeignKey("Role")]
        public long RoleId { get; set; }
        public virtual Role Role { get; set; } = null!;
        
        [Column("AssignedDate")]
        public DateTime AssignedDate { get; set; } = DateTime.UtcNow;
        
        [Column("ExpiryDate")]
        public DateTime? ExpiryDate { get; set; }
        
        [Column("AssignedByUserId")]
        public long? AssignedByUserId { get; set; }
        
        [Column("Notes")]
        [MaxLength(500)]
        public string? Notes { get; set; }
        
        // Computed Properties
        [NotMapped]
        public bool IsExpired => ExpiryDate.HasValue && ExpiryDate.Value < DateTime.UtcNow;
        
        /// <summary>
        /// User role active status
        /// </summary>
        public new bool IsActive { get; set; } = true;
    }
}