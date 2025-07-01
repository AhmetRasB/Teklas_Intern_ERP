using System;

namespace Teklas_Intern_ERP.Entities.ProductionManagement
{
    public class ProductionConfirmation
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public decimal ConfirmedQuantity { get; set; }
        public decimal ScrapQuantity { get; set; }
        public DateTime ConfirmationDate { get; set; }
        public string ConfirmedBy { get; set; }
        public string Description { get; set; }
        public DateTime CreatedDate { get; set; }
    }
} 