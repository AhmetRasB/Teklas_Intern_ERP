using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.DataAccess.WarehouseManagement
{
    public class WarehouseRepository
    {
        private readonly AppDbContext _context;
        public WarehouseRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Warehouse> GetAll() => _context.Warehouses.ToList();
        public Warehouse GetById(int id) => _context.Warehouses.Find(id);
        public Warehouse Add(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            _context.SaveChanges();
            return warehouse;
        }
        public bool Update(Warehouse warehouse)
        {
            var existing = _context.Warehouses.Find(warehouse.Id);
            if (existing == null) return false;
            existing.WarehouseName = warehouse.WarehouseName;
            existing.WarehouseCode = warehouse.WarehouseCode;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var warehouse = _context.Warehouses.Find(id);
            if (warehouse == null) return false;
            _context.Warehouses.Remove(warehouse);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<Warehouse>> GetAllAsync() => await _context.Warehouses.ToListAsync();
        public async Task<Warehouse> GetByIdAsync(int id) => await _context.Warehouses.FindAsync(id);
        public async Task<Warehouse> AddAsync(Warehouse warehouse)
        {
            _context.Warehouses.Add(warehouse);
            await _context.SaveChangesAsync();
            return warehouse;
        }
        public async Task<bool> UpdateAsync(Warehouse warehouse)
        {
            var existing = await _context.Warehouses.FindAsync(warehouse.Id);
            if (existing == null) return false;
            existing.WarehouseName = warehouse.WarehouseName;
            existing.WarehouseCode = warehouse.WarehouseCode;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var warehouse = await _context.Warehouses.FindAsync(id);
            if (warehouse == null) return false;
            _context.Warehouses.Remove(warehouse);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 