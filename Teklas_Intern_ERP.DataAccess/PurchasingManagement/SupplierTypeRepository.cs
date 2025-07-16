using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.Entities.PurchasingManagement;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public sealed class SupplierTypeRepository : BaseRepository<SupplierType>, ISupplierTypeRepository
    {
        public SupplierTypeRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<List<SupplierType>> SearchAsync(string searchTerm, params string[] searchFields)
        {
            return await _dbSet
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && 
                           (e.Name.Contains(searchTerm) || 
                            (e.Description != null && e.Description.Contains(searchTerm))))
                .ToListAsync();
        }

        public async Task<List<SupplierType>> GetActiveSupplierTypesAsync()
        {
            return await _dbSet
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.IsActive)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name, long excludeId = 0)
        {
            return await _dbSet
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.Name == name && e.Id != excludeId)
                .AnyAsync();
        }
    }
} 