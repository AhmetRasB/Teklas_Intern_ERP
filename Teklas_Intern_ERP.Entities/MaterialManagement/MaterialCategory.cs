using System;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    public class MaterialCategory
    {
        public int Id { get; set; }
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public int? ParentCategoryId { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 