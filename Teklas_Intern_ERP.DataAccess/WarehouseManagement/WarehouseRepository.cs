using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.WarehouseManagement;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public class WarehouseRepository
    {
        private static List<Warehouse> _warehouses = new List<Warehouse>();
        private static int _nextId = 1;

        public List<Warehouse> GetAll() => _warehouses;
        public Warehouse GetById(int id) => _warehouses.FirstOrDefault(x => x.Id == id);
        public Warehouse Add(Warehouse warehouse)
        {
            warehouse.Id = _nextId++;
            _warehouses.Add(warehouse);
            return warehouse;
        }
        public bool Update(Warehouse warehouse)
        {
            var existing = GetById(warehouse.Id);
            if (existing == null) return false;
            existing.Name = warehouse.Name;
            existing.Code = warehouse.Code;
            return true;
        }
        public bool Delete(int id)
        {
            var warehouse = GetById(id);
            if (warehouse == null) return false;
            _warehouses.Remove(warehouse);
            return true;
        }
    }
} 