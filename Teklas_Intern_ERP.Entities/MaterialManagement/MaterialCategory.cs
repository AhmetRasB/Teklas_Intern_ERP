using System.Collections.Generic;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    public class MaterialCategory : AuditEntity
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public long? ParentCategoryId { get; set; }
        public MaterialCategory? ParentCategory { get; set; }
        public ICollection<MaterialCategory> SubCategories { get; set; } = new List<MaterialCategory>();
    }
} 