using System;

namespace Teklas_Intern_ERP.Entities.ProductionManagement
{
    public class BillOfMaterials
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal ScrapRate { get; set; }
        public int Sequence { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 