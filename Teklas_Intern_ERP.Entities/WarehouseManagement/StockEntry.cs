namespace Teklas_Intern_ERP.Entities.WarehouseManagement
{
    public class StockEntry
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public int Quantity { get; set; }
        public DateTime EntryDate { get; set; }
    }
} 