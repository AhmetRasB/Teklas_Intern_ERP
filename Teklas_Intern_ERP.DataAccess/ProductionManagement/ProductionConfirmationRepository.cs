using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement;

public class ProductionConfirmationRepository : BaseRepository<ProductionConfirmation>, IProductionConfirmationRepository
{
    public ProductionConfirmationRepository(AppDbContext context) : base(context) { }

    public async Task<ProductionConfirmation?> GetWithConsumptionsAsync(long id)
    {
        return await _context.ProductionConfirmations
            .Include(p => p.Consumptions)
            .FirstOrDefaultAsync(p => p.ConfirmationId == id && !p.IsDeleted);
    }

    public async Task<List<ProductionConfirmation>> GetAllWithConsumptionsAsync()
    {
        return await _context.ProductionConfirmations
            .Include(p => p.Consumptions)
            .Where(p => !p.IsDeleted)
            .ToListAsync();
    }

    public async Task<ProductionConfirmation?> GetByIdForDeleteAsync(long id)
    {
        return await _context.ProductionConfirmations
            .FirstOrDefaultAsync(p => p.ConfirmationId == id && !p.IsDeleted);
    }

    public async Task<ProductionConfirmation?> GetDeletedByIdAsync(long id)
    {
        return await _context.ProductionConfirmations
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(p => p.ConfirmationId == id && p.IsDeleted);
    }

    public async Task<List<ProductionConfirmation>> GetAllDeletedAsync()
    {
        return await _context.ProductionConfirmations
            .IgnoreQueryFilters()
            .Include(p => p.Consumptions)
            .Where(p => p.IsDeleted)
            .ToListAsync();
    }
} 