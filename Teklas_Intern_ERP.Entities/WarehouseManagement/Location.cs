using System;

namespace Teklas_Intern_ERP.Entities.WarehouseManagement
{
    public class Location
    {
        public int Id { get; set; }
        public int WarehouseId { get; set; }
        public string LocationCode { get; set; }
        public string LocationName { get; set; }
        public string Description { get; set; }
        public decimal Capacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 