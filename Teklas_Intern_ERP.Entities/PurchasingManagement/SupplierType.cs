using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities;

namespace Teklas_Intern_ERP.Entities.PurchasingManagement
{
    public sealed class SupplierType : AuditEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500)]
        public string? Description { get; set; }
        
        [Required]
        public bool IsActive { get; set; } = true;
    }
} 