using System;
using Teklas_Intern_ERP.Entities.Interfaces;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    public class MaterialMovement : AuditEntity
    {
        public long MaterialId { get; set; }
        public MaterialCard Material { get; set; }
        public MovementType MovementType { get; set; }
        public decimal Quantity { get; set; }
        public string Unit { get; set; }
        public DateTime MovementDate { get; set; }
        public string? Warehouse { get; set; }
        public string? Location { get; set; }
        public string? Description { get; set; }
    }

    public enum MovementType
    {
        Inbound = 1,
        Outbound = 2,
        Transfer = 3,
        Adjustment = 4
    }
} 