using System;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.Entities
{
    public abstract class AuditEntity : IEntity
    {
        public long Id { get; set; }
        public StatusType Status { get; set; } = StatusType.Active;
        public DateTime CreateDate { get; set; }
        public long CreateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public long? UpdateUserId { get; set; }
        public DateTime? DeleteDate { get; set; }
        public long? DeleteUserId { get; set; }
        public bool IsDeleted { get; set; } = false;
    }

    public enum StatusType
    {
        Active = 1,
        Passive = 2,
        Blocked = 3,
        Frozen = 4
    }
} 