using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.Entities.SalesManagement;

namespace Teklas_Intern_ERP.DataAccess.SalesManagement
{
    public sealed class CustomerOrderRepository : BaseRepository<CustomerOrder>, ICustomerOrderRepository
    {
        public CustomerOrderRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<List<CustomerOrder>> SearchAsync(string searchTerm, params string[] searchFields)
        {
            return await _dbSet
                .Include(co => co.Customer)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && 
                           (e.OrderNumber.Contains(searchTerm) || 
                            (e.Description != null && e.Description.Contains(searchTerm))))
                .ToListAsync();
        }

        public async Task<List<CustomerOrder>> GetOrdersByCustomerAsync(long customerId)
        {
            return await _dbSet
                .Include(co => co.Customer)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<CustomerOrder>> GetOrdersByStatusAsync(string status)
        {
            return await _dbSet
                .Include(co => co.Customer)
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.Status == status)
                .ToListAsync();
        }

        public async Task<List<CustomerOrder>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _dbSet
                .Include(co => co.Customer)
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