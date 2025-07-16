using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Entities.ProductionManagment;

public class WorkOrder : AuditEntity
{
    [Key]
    public long WorkOrderId { get; set; }

    public long BOMHeaderId { get; set; } // Ürün ağacı (BOM) referansı
    public virtual BOMHeader BOMHeader { get; set; } = null!;

    public long MaterialCardId { get; set; } // Üretilecek ürün

    public decimal PlannedQuantity { get; set; }
    public DateTime PlannedStartDate { get; set; }
    public DateTime PlannedEndDate { get; set; }

    // Status is inherited from AuditEntity as StatusType enum

    public string? ReferenceNumber { get; set; }

    public virtual MaterialCard MaterialCard { get; set; } = null!;
    public virtual ICollection<WorkOrderOperation> Operations { get; set; } = new HashSet<WorkOrderOperation>();
}
