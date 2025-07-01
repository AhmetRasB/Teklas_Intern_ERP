using System;

namespace Teklas_Intern_ERP.Entities.WarehouseManagement
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string WarehouseCode { get; set; }
        public string WarehouseName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ResponsiblePerson { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public decimal Capacity { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 