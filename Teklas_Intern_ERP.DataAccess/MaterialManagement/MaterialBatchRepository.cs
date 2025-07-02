using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.MaterialManagement;

namespace Teklas_Intern_ERP.DataAccess.MaterialManagement
{
    public class MaterialBatchRepository
    {
        private readonly AppDbContext _context;
        public MaterialBatchRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<MaterialBatch>> GetAllAsync() => await _context.MaterialBatches.ToListAsync();
        public async Task<MaterialBatch> GetByIdAsync(int id) => await _context.MaterialBatches.FindAsync(id);
        public async Task<MaterialBatch> AddAsync(MaterialBatch batch)
        {
            _context.MaterialBatches.Add(batch);
            await _context.SaveChangesAsync();
            return batch;
        }
        public async Task<bool> UpdateAsync(MaterialBatch batch)
        {
            var existing = await _context.MaterialBatches.FindAsync(batch.Id);
            if (existing == null) return false;
            existing.MaterialId = batch.MaterialId;
            existing.BatchNo = batch.BatchNo;
            existing.ExpiryDate = batch.ExpiryDate;
            existing.Quantity = batch.Quantity;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var batch = await _context.MaterialBatches.FindAsync(id);
            if (batch == null) return false;
            _context.MaterialBatches.Remove(batch);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 