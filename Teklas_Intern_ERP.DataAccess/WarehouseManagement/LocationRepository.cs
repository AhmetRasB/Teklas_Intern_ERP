using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public class LocationRepository
    {
        private readonly AppDbContext _context;
        public LocationRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Location> GetAll() => _context.Locations.ToList();
        public Location GetById(int id) => _context.Locations.Find(id);
        public Location Add(Location location)
        {
            _context.Locations.Add(location);
            _context.SaveChanges();
            return location;
        }
        public bool Update(Location location)
        {
            var existing = _context.Locations.Find(location.Id);
            if (existing == null) return false;
            existing.LocationName = location.LocationName;
            existing.LocationCode = location.LocationCode;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var location = _context.Locations.Find(id);
            if (location == null) return false;
            _context.Locations.Remove(location);
            _context.SaveChanges();
            return true;
        }
    }
} 