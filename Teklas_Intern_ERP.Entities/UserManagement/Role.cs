using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace Teklas_Intern_ERP.Entities.UserManagement
{
    [Table("Roles")]
    public class Role : AuditEntity
    {
        [Required]
        [Column("Name")]
        [MaxLength(50)]
        public string Name { get; set; } = string.Empty;
        
        [Required]
        [Column("DisplayName")]
        [MaxLength(100)]
        public string DisplayName { get; set; } = string.Empty;
        
        [Column("Description")]
        [MaxLength(500)]
        public string? Description { get; set; }
        
        [Column("Permissions")]
        [MaxLength(4000)]
        public string? Permissions { get; set; } // JSON format for permissions
        
        [Column("Color")]
        [MaxLength(7)] // For hex color codes like #FF5733
        public string? Color { get; set; }
        
        [Column("IsSystemRole")]
        public bool IsSystemRole { get; set; } = false; // System roles cannot be deleted
        
        [Column("Priority")]
        public int Priority { get; set; } = 0; // Higher number = higher priority
        
        // Navigation Properties
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
        
        // Computed Properties
        /// <summary>
        /// Role active status
        /// </summary>
        public new bool IsActive { get; set; } = true;
        
        [NotMapped]
        public int UserCount => UserRoles?.Count ?? 0;
    }
}