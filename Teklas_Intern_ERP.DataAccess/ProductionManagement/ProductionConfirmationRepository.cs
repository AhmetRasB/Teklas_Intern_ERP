using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.DataAccess;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement
{
    public class ProductionConfirmationRepository
    {
        private readonly AppDbContext _context;
        public ProductionConfirmationRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<ProductionConfirmation> GetAll() => _context.ProductionConfirmations.ToList();
        public ProductionConfirmation GetById(int id) => _context.ProductionConfirmations.Find(id);
        public ProductionConfirmation Add(ProductionConfirmation confirmation)
        {
            _context.ProductionConfirmations.Add(confirmation);
            _context.SaveChanges();
            return confirmation;
        }
        public bool Update(ProductionConfirmation confirmation)
        {
            var existing = _context.ProductionConfirmations.Find(confirmation.Id);
            if (existing == null) return false;
            existing.WorkOrderId = confirmation.WorkOrderId;
            existing.ConfirmationDate = confirmation.ConfirmationDate;
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var confirmation = _context.ProductionConfirmations.Find(id);
            if (confirmation == null) return false;
            _context.ProductionConfirmations.Remove(confirmation);
            _context.SaveChanges();
            return true;
        }

        public async Task<List<ProductionConfirmation>> GetAllAsync() => await _context.ProductionConfirmations.ToListAsync();
        public async Task<ProductionConfirmation> GetByIdAsync(int id) => await _context.ProductionConfirmations.FindAsync(id);
        public async Task<ProductionConfirmation> AddAsync(ProductionConfirmation confirmation)
        {
            _context.ProductionConfirmations.Add(confirmation);
            await _context.SaveChangesAsync();
            return confirmation;
        }
        public async Task<bool> UpdateAsync(ProductionConfirmation confirmation)
        {
            var existing = await _context.ProductionConfirmations.FindAsync(confirmation.Id);
            if (existing == null) return false;
            existing.WorkOrderId = confirmation.WorkOrderId;
            existing.ConfirmationDate = confirmation.ConfirmationDate;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var confirmation = await _context.ProductionConfirmations.FindAsync(id);
            if (confirmation == null) return false;
            _context.ProductionConfirmations.Remove(confirmation);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 