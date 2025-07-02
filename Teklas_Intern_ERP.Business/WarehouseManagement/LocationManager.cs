using Teklas_Intern_ERP.DataAccess.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.WarehouseManagement
{
    public class LocationManager
    {
        private readonly LocationRepository _repo;
        public LocationManager(LocationRepository repo)
        {
            _repo = repo;
        }

        public List<Location> GetAll() => _repo.GetAll();
        public Location GetById(int id) => _repo.GetById(id);
        public Location Add(Location location) => _repo.Add(location);
        public bool Update(Location location) => _repo.Update(location);
        public bool Delete(int id) => _repo.Delete(id);

        public async Task<List<Location>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Location> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<Location> AddAsync(Location location) => await _repo.AddAsync(location);
        public async Task<bool> UpdateAsync(Location location) => await _repo.UpdateAsync(location);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 