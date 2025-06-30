using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;

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
            existing.Name = bom.Name;
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
    }
} 