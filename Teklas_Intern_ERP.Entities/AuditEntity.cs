using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.Entities
{
    public abstract class AuditEntity : IEntity
    {
        [Key]
        public long Id { get; set; }
        
        [Required]
        public StatusType Status { get; set; } = StatusType.Active;
        
        // Audit Trail Fields
        [Required]
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        
        [Required]
        public long CreateUserId { get; set; }
        
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserId { get; set; }
        
        public DateTime? DeleteDate { get; set; }
        public long? DeleteUserId { get; set; }
        
        public bool IsDeleted { get; set; } = false;
        
        // Business Logic Helpers
        [NotMapped]
        public bool IsActive => Status == StatusType.Active && !IsDeleted;
    }

    public enum StatusType
    {
        Active = 1,
        Passive = 2,
        Blocked = 3,
        Frozen = 4
    }
} 