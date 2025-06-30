namespace Teklas_Intern_ERP.Entities.ProductionManagement
{
    public class ProductionConfirmation
    {
        public int Id { get; set; }
        public int WorkOrderId { get; set; }
        public DateTime ConfirmationDate { get; set; }
    }
} 