using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities;

namespace Teklas_Intern_ERP.Entities.SalesManagement
{
    public sealed class Customer : AuditEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(200)]
        public string? Address { get; set; }
        
        [StringLength(50)]
        public string? Phone { get; set; }
        
        [StringLength(100)]
        [EmailAddress]
        public string? Email { get; set; }
        
        [StringLength(50)]
        public string? TaxNumber { get; set; }
        
        [StringLength(50)]
        public string? ContactPerson { get; set; }
        
        [Required]
        public bool IsActive { get; set; } = true;
    }
} 