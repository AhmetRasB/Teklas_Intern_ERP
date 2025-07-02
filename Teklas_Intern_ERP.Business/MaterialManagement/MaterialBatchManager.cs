using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;
using Teklas_Intern_ERP.DataAccess.MaterialManagement;

namespace Teklas_Intern_ERP.Business.MaterialManagement
{
    public class MaterialBatchManager
    {
        private readonly AppDbContext _context;
        private readonly MaterialBatchRepository _repo;
        public MaterialBatchManager(AppDbContext context, MaterialBatchRepository repo)
        {
            _context = context;
            _repo = repo;
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

        public async Task<List<MaterialBatch>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<MaterialBatch> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<MaterialBatch> AddAsync(MaterialBatch batch) => await _repo.AddAsync(batch);
        public async Task<bool> UpdateAsync(MaterialBatch batch) => await _repo.UpdateAsync(batch);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 