using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Teklas_Intern_ERP.Entities.UserManagement
{
    [Table("UserTableColumnPreferences")]
    public sealed class UserTableColumnPreference
    {
        [Key]
        public int Id { get; set; }
        public int UserId { get; set; }
        [Required, MaxLength(100)]
        public string TableKey { get; set; } = string.Empty;
        [Required]
        public string ColumnsJson { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
} 