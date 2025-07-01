using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;

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
    }
} 