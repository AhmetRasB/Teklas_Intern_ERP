using System.ComponentModel.DataAnnotations;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.Entities.ProductionManagment;

public class MaterialConsumption : AuditEntity
{
    [Key]
    public long ConsumptionId { get; set; }

    public long ConfirmationId { get; set; }
    public long MaterialCardId { get; set; }

    public decimal QuantityUsed { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal? TotalAmount { get; set; }

    [StringLength(100)]
    public string? BatchNumber { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    public virtual ProductionConfirmation ProductionConfirmation { get; set; } = null!;
    public virtual MaterialCard MaterialCard { get; set; } = null!;
}