using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    public class BillOfMaterialsRepository
    {
        private readonly AppDbContext _context;
        public BillOfMaterialsRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<BillOfMaterials> GetAll() => _context.BillOfMaterials.ToList();
        public BillOfMaterials GetById(int id) => _context.BillOfMaterials.Find(id);
        public BillOfMaterials Add(BillOfMaterials bom)
        {
            _context.BillOfMaterials.Add(bom);
            _context.SaveChanges();
            return bom;
        }
        public bool Update(BillOfMaterials bom)
        {
            var existing = _context.BillOfMaterials.Find(bom.Id);
            if (existing == null) return false;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var bom = _context.BillOfMaterials.Find(id);
            if (bom == null) return false;
            _context.BillOfMaterials.Remove(bom);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<BillOfMaterials>> GetAllAsync() => await _context.BillOfMaterials.ToListAsync();
        public async Task<BillOfMaterials> GetByIdAsync(int id) => await _context.BillOfMaterials.FindAsync(id);
        public async Task<BillOfMaterials> AddAsync(BillOfMaterials bom)
        {
            _context.BillOfMaterials.Add(bom);
            await _context.SaveChangesAsync();
            return bom;
        }
        public async Task<bool> UpdateAsync(BillOfMaterials bom)
        {
            var existing = await _context.BillOfMaterials.FindAsync(bom.Id);
            if (existing == null) return false;
            // Gerekli alanlar güncellenebilir, örn:
            // existing.ProductId = bom.ProductId;
            // existing.MaterialId = bom.MaterialId;
            // existing.Quantity = bom.Quantity;
            // existing.UnitOfMeasure = bom.UnitOfMeasure;
            // existing.ScrapRate = bom.ScrapRate;
            // existing.Sequence = bom.Sequence;
            // existing.UpdatedDate = DateTime.Now;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var bom = await _context.BillOfMaterials.FindAsync(id);
            if (bom == null) return false;
            _context.BillOfMaterials.Remove(bom);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 