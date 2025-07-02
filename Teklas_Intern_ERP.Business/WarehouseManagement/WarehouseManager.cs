using Teklas_Intern_ERP.DataAccess.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.WarehouseManagement
{
    public class WarehouseManager
    {
        private readonly WarehouseRepository _repo;
        public WarehouseManager(WarehouseRepository repo)
        {
            _repo = repo;
        }

        public List<Warehouse> GetAll() => _repo.GetAll();
        public Warehouse GetById(int id) => _repo.GetById(id);
        public Warehouse Add(Warehouse warehouse) => _repo.Add(warehouse);
        public bool Update(Warehouse warehouse) => _repo.Update(warehouse);
        public bool Delete(int id) => _repo.Delete(id);

        public async Task<List<Warehouse>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Warehouse> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<Warehouse> AddAsync(Warehouse warehouse) => await _repo.AddAsync(warehouse);
        public async Task<bool> UpdateAsync(Warehouse warehouse) => await _repo.UpdateAsync(warehouse);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 