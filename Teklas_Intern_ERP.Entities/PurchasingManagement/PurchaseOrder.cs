using System;

namespace Teklas_Intern_ERP.Entities.PurchasingManagement
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PurchaseOrderNo { get; set; }
        public int SupplierId { get; set; }
        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string Currency { get; set; }
        public string PaymentTerms { get; set; }
        public string DeliveryAddress { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 