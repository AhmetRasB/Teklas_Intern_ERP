using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement
{
    public class MaterialCategoryRepository
    {
        private readonly AppDbContext _context;
        public MaterialCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<MaterialCategory> GetAll() => _context.MaterialCategories.ToList();
        public MaterialCategory GetById(int id) => _context.MaterialCategories.Find(id);
        public MaterialCategory Add(MaterialCategory category)
        {
            _context.MaterialCategories.Add(category);
            _context.SaveChanges();
            return category;
        }
        public bool Update(MaterialCategory category)
        {
            var existing = _context.MaterialCategories.Find(category.Id);
            if (existing == null) return false;
            existing.CategoryName = category.CategoryName;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var category = _context.MaterialCategories.Find(id);
            if (category == null) return false;
            _context.MaterialCategories.Remove(category);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<MaterialCategory>> GetAllAsync() => await _context.MaterialCategories.ToListAsync();
        public async Task<MaterialCategory> GetByIdAsync(int id) => await _context.MaterialCategories.FindAsync(id);
        public async Task<MaterialCategory> AddAsync(MaterialCategory category)
        {
            _context.MaterialCategories.Add(category);
            await _context.SaveChangesAsync();
            return category;
        }
        public async Task<bool> UpdateAsync(MaterialCategory category)
        {
            var existing = await _context.MaterialCategories.FindAsync(category.Id);
            if (existing == null) return false;
            existing.CategoryName = category.CategoryName;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var category = await _context.MaterialCategories.FindAsync(id);
            if (category == null) return false;
            _context.MaterialCategories.Remove(category);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 