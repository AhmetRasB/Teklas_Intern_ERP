using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Entities.ProductionManagment;

public class BOMHeader : AuditEntity
{
    [Key]
    public long BOMHeaderId { get; set; }

    [Required]
    public long ParentMaterialCardId { get; set; } // Ana ürün (Üretilecek)

    [StringLength(20)]
    public string Version { get; set; } = "1.0";

    public DateTime ValidFrom { get; set; } = DateTime.UtcNow;
    public DateTime? ValidTo { get; set; }

    public decimal? StandardCost { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    public virtual MaterialCard ParentMaterialCard { get; set; } = null!;
    public virtual ICollection<BOMItem> BOMItems { get; set; } = new HashSet<BOMItem>();
}