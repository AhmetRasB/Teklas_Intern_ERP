using Teklas_Intern_ERP.DataAccess.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.WarehouseManagement
{
    public class WarehouseManager
    {
        private readonly WarehouseRepository _repo;
        public WarehouseManager()
        {
            _repo = new WarehouseRepository();
        }

        public List<Warehouse> GetAll() => _repo.GetAll();
        public Warehouse GetById(int id) => _repo.GetById(id);
        public Warehouse Add(Warehouse warehouse) => _repo.Add(warehouse);
        public bool Update(Warehouse warehouse) => _repo.Update(warehouse);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 