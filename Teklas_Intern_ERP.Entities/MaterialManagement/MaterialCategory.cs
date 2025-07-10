using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    /// <summary>
    /// Material Category Entity - Enterprise Resource Planning
    /// Represents material categorization in the system
    /// </summary>
    public class MaterialCategory : AuditEntity
    {
        public MaterialCategory()
        {
            MaterialCards = new HashSet<MaterialCard>();
        }

        /// <summary>
        /// Unique category code
        /// </summary>
        [Required]
        [StringLength(50)]
        public string CategoryCode { get; set; } = string.Empty;

        /// <summary>
        /// Category name
        /// </summary>
        [Required]
        [StringLength(200)]
        public string CategoryName { get; set; } = string.Empty;

        /// <summary>
        /// Category description
        /// </summary>
        [StringLength(500)]
        public string? Description { get; set; }

        /// <summary>
        /// Parent category ID for hierarchical structure
        /// </summary>
        public long? ParentCategoryId { get; set; }

        /// <summary>
        /// Category level in hierarchy (0 = root)
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Category path for easy hierarchy navigation
        /// </summary>
        [StringLength(1000)]
        public string? CategoryPath { get; set; }

        /// <summary>
        /// Display order for sorting
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Category status (Active, Inactive)
        /// </summary>
        [StringLength(20)]
        public new string? Status { get; set; }

        #region Navigation Properties

        /// <summary>
        /// Parent category navigation property
        /// </summary>
        public virtual MaterialCategory? ParentCategory { get; set; }

        /// <summary>
        /// Child categories collection
        /// </summary>
        public virtual ICollection<MaterialCategory> ChildCategories { get; set; } = new HashSet<MaterialCategory>();

        /// <summary>
        /// Material cards in this category
        /// </summary>
        public virtual ICollection<MaterialCard> MaterialCards { get; set; }

        #endregion
    }
} 