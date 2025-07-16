using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.Entities.PurchasingManagement;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public sealed class SupplierRepository : BaseRepository<Supplier>, ISupplierRepository
    {
        public SupplierRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<List<Supplier>> SearchAsync(string searchTerm, params string[] searchFields)
        {
            return await _dbSet
                .Include(s => s.SupplierType)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && 
                           (e.Name.Contains(searchTerm) || 
                            e.ContactPerson.Contains(searchTerm) || 
                            (e.Email != null && e.Email.Contains(searchTerm))))
                .ToListAsync();
        }

        public async Task<List<Supplier>> GetActiveSuppliersAsync()
        {
            return await _dbSet
                .Include(s => s.SupplierType)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.IsActive)
                .ToListAsync();
        }

        public async Task<List<Supplier>> GetSuppliersByTypeAsync(long supplierTypeId)
        {
            return await _dbSet
                .Include(s => s.SupplierType)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.SupplierTypeId == supplierTypeId)
                .ToListAsync();
        }

        public async Task<bool> ExistsByNameAsync(string name, long excludeId = 0)
        {
            return await _dbSet
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.Name == name && e.Id != excludeId)
                .AnyAsync();
        }

        public async Task<bool> ExistsByTaxNumberAsync(string taxNumber, long excludeId = 0)
        {
            return await _dbSet
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.TaxNumber == taxNumber && e.Id != excludeId)
                .AnyAsync();
        }
    }
} 