using System.ComponentModel.DataAnnotations;

namespace Teklas_Intern_ERP.Entities.ProductionManagment;

public class ProductionConfirmation : AuditEntity
{
    [Key]
    public long ConfirmationId { get; set; }

    public long WorkOrderId { get; set; }

    public DateTime ConfirmationDate { get; set; }
    public decimal QuantityProduced { get; set; }
    public decimal? QuantityScrapped { get; set; }
    public decimal? LaborHoursUsed { get; set; }

    [StringLength(100)]
    public string? PerformedBy { get; set; }

    public virtual WorkOrder WorkOrder { get; set; } = null!;
    public virtual ICollection<MaterialConsumption> Consumptions { get; set; } = new HashSet<MaterialConsumption>();
}