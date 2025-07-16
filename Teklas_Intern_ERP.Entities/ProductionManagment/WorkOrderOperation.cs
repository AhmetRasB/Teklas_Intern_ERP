using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.ProductionManagment;

public class WorkOrderOperation : AuditEntity
{
    [Key]
    public long OperationId { get; set; }

    public long WorkOrderId { get; set; }

    [StringLength(100)]
    public string OperationName { get; set; } = string.Empty;

    public int Sequence { get; set; }
    public decimal PlannedHours { get; set; }

    [StringLength(100)]
    public string? Resource { get; set; }

    public virtual WorkOrder WorkOrder { get; set; } = null!;
}