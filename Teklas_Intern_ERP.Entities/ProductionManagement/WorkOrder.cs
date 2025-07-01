using System;

namespace Teklas_Intern_ERP.Entities.ProductionManagement
{
    public class WorkOrder
    {
        public int Id { get; set; }
        public string WorkOrderNo { get; set; }
        public int ProductId { get; set; }
        public DateTime PlannedStartDate { get; set; }
        public DateTime PlannedEndDate { get; set; }
        public DateTime? ActualStartDate { get; set; }
        public DateTime? ActualEndDate { get; set; }
        public decimal Quantity { get; set; }
        public string Status { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Description { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 