using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialBatchManager
    {
        private readonly AppDbContext _context;
        public MaterialBatchManager(AppDbContext context)
        {
            _context = context;
        }

        public List<MaterialBatch> GetAll() => _context.MaterialBatches.ToList();
        public MaterialBatch GetById(int id) => _context.MaterialBatches.Find(id);
        public MaterialBatch Add(MaterialBatch batch)
        {
            _context.MaterialBatches.Add(batch);
            _context.SaveChanges();
            return batch;
        }
        public bool Update(MaterialBatch batch)
        {
            var existing = _context.MaterialBatches.Find(batch.Id);
            if (existing == null) return false;
            existing.MaterialId = batch.MaterialId;
            existing.BatchNo = batch.BatchNo;
            existing.ExpiryDate = batch.ExpiryDate;
            existing.Quantity = batch.Quantity;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var batch = _context.MaterialBatches.Find(id);
            if (batch == null) return false;
            _context.MaterialBatches.Remove(batch);
            _context.SaveChanges();
            return true;
        }
    }
} 