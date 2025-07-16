using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Entities.ProductionManagment;

public class BOMItem : AuditEntity
{
    [Key]
    public long BOMItemId { get; set; }

    public long BOMHeaderId { get; set; }
    public long ComponentMaterialCardId { get; set; } // Alt parça

    public decimal Quantity { get; set; }
    public decimal? ScrapRate { get; set; } // Fire oranı

    public virtual BOMHeader BOMHeader { get; set; } = null!;
    public virtual MaterialCard ComponentMaterialCard { get; set; } = null!;
}