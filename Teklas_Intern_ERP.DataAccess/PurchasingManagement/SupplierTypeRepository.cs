using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public class SupplierTypeRepository
    {
        private readonly AppDbContext _context;
        public SupplierTypeRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<SupplierType> GetAll() => _context.SupplierTypes.ToList();
        public SupplierType GetById(int id) => _context.SupplierTypes.Find(id);
        public SupplierType Add(SupplierType type)
        {
            _context.SupplierTypes.Add(type);
            _context.SaveChanges();
            return type;
        }
        public bool Update(SupplierType type)
        {
            var existing = _context.SupplierTypes.Find(type.Id);
            if (existing == null) return false;
            existing.TypeName = type.TypeName;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var type = _context.SupplierTypes.Find(id);
            if (type == null) return false;
            _context.SupplierTypes.Remove(type);
            _context.SaveChanges();
            return true;
        }
    }
} 