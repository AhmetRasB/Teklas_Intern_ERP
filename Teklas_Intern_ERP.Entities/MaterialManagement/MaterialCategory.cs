using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    [Table("MaterialCategories")]

    public class MaterialCategory : AuditEntity
    {
        [Required]
        public string Code { get; set; } = string.Empty;
        
        [Required]
        public string Name { get; set; } = string.Empty;
        
        public string? Description { get; set; }
        
        // Hierarchy Support
        [ForeignKey("ParentCategory")]
        public long? ParentCategoryId { get; set; }
        public MaterialCategory? ParentCategory { get; set; }
        
        // Navigation Properties
        public ICollection<MaterialCategory> SubCategories { get; set; } = new List<MaterialCategory>();
        public ICollection<MaterialCard> MaterialCards { get; set; } = new List<MaterialCard>();
    }
} 