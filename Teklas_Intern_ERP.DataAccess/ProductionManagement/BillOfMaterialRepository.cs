using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Teklas_Intern_ERP.DataAccess.ProductionManagement;

public class BillOfMaterialRepository : BaseRepository<BOMHeader>, IBillOfMaterialRepository
{
    public BillOfMaterialRepository(AppDbContext context) : base(context) { }

    public async Task<BOMHeader?> GetWithItemsAsync(long id)
    {
        return await _context.BOMHeaders
            .Include(b => b.BOMItems)
            .FirstOrDefaultAsync(b => b.BOMHeaderId == id && !b.IsDeleted);
    }

    public async Task<List<BOMHeader>> GetAllWithItemsAsync()
    {
        return await _context.BOMHeaders
            .Include(b => b.BOMItems)
            .Where(b => !b.IsDeleted)
            .ToListAsync();
    }

    public async Task<BOMHeader?> GetByIdForDeleteAsync(long bomHeaderId)
    {
        return await _context.BOMHeaders
            .IgnoreQueryFilters()
            .FirstOrDefaultAsync(b => b.BOMHeaderId == bomHeaderId);
    }

    public async Task<List<BOMHeader>> GetDeletedAsync()
    {
        return await _context.BOMHeaders
            .IgnoreQueryFilters()
            .Include(b => b.BOMItems)
            .Where(b => b.IsDeleted)
            .ToListAsync();
    }
} 