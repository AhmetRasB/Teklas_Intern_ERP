using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

        public async Task<List<Location>> GetAllAsync() => await _context.Locations.ToListAsync();
        public async Task<Location> GetByIdAsync(int id) => await _context.Locations.FindAsync(id);
        public async Task<Location> AddAsync(Location location)
        {
            _context.Locations.Add(location);
            await _context.SaveChangesAsync();
            return location;
        }
        public async Task<bool> UpdateAsync(Location location)
        {
            var existing = await _context.Locations.FindAsync(location.Id);
            if (existing == null) return false;
            existing.LocationName = location.LocationName;
            existing.LocationCode = location.LocationCode;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var location = await _context.Locations.FindAsync(id);
            if (location == null) return false;
            _context.Locations.Remove(location);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 