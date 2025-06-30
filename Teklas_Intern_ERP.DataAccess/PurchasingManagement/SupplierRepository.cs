using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public class SupplierRepository
    {
        private readonly AppDbContext _context;
        public SupplierRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Supplier> GetAll() => _context.Suppliers.ToList();
        public Supplier GetById(int id) => _context.Suppliers.Find(id);
        public Supplier Add(Supplier supplier)
        {
            _context.Suppliers.Add(supplier);
            _context.SaveChanges();
            return supplier;
        }
        public bool Update(Supplier supplier)
        {
            var existing = _context.Suppliers.Find(supplier.Id);
            if (existing == null) return false;
            existing.Name = supplier.Name;
            existing.SupplierTypeId = supplier.SupplierTypeId;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var supplier = _context.Suppliers.Find(id);
            if (supplier == null) return false;
            _context.Suppliers.Remove(supplier);
            _context.SaveChanges();
            return true;
        }
    }
} 