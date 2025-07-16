using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.Entities.PurchasingManagement;

namespace Teklas_Intern_ERP.DataAccess.PurchasingManagement
{
    public sealed class PurchaseOrderRepository : BaseRepository<PurchaseOrder>, IPurchaseOrderRepository
    {
        public PurchaseOrderRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<List<PurchaseOrder>> SearchAsync(string searchTerm, params string[] searchFields)
        {
            return await _dbSet
                .Include(po => po.Supplier)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && 
                           (e.OrderNumber.Contains(searchTerm) || 
                            (e.Description != null && e.Description.Contains(searchTerm))))
                .ToListAsync();
        }

        public async Task<List<PurchaseOrder>> GetOrdersBySupplierAsync(long supplierId)
        {
            return await _dbSet
                .Include(po => po.Supplier)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.SupplierId == supplierId)
                .ToListAsync();
        }

        public async Task<List<PurchaseOrder>> GetOrdersByStatusAsync(string status)
        {
            return await _dbSet
                .Include(po => po.Supplier)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.Status == status)
                .ToListAsync();
        }

        public async Task<List<PurchaseOrder>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(po => po.Supplier)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.OrderDate >= startDate && e.OrderDate <= endDate)
                .ToListAsync();
        }

        public async Task<bool> ExistsByOrderNumberAsync(string orderNumber, long excludeId = 0)
        {
            return await _dbSet
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.OrderNumber == orderNumber && e.Id != excludeId)
                .AnyAsync();
        }
    }
} 