using Teklas_Intern_ERP.DataAccess.WarehouseManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using System.Collections.Generic;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.WarehouseManagement
{
    public class LocationManager
    {
        private readonly LocationRepository _repo;
        public LocationManager(AppDbContext context)
        {
            _repo = new LocationRepository(context);
        }

        public List<Location> GetAll() => _repo.GetAll();
        public Location GetById(int id) => _repo.GetById(id);
        public Location Add(Location location) => _repo.Add(location);
        public bool Update(Location location) => _repo.Update(location);
        public bool Delete(int id) => _repo.Delete(id);
    }
} 