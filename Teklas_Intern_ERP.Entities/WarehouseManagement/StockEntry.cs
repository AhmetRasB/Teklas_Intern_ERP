using System;

namespace Teklas_Intern_ERP.Entities.WarehouseManagement
{
    public class StockEntry
    {
        public int Id { get; set; }
        public int MaterialId { get; set; }
        public int WarehouseId { get; set; }
        public int LocationId { get; set; }
        public decimal Quantity { get; set; }
        public DateTime EntryDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string BatchNo { get; set; }
        public string LotNo { get; set; }
        public int? SupplierId { get; set; }
        public int? PurchaseOrderId { get; set; }
        public decimal UnitPrice { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }
} 