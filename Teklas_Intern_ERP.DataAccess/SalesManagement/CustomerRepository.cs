using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.DataAccess.Repositories;
using Teklas_Intern_ERP.Entities.SalesManagement;

namespace Teklas_Intern_ERP.DataAccess.SalesManagement
{
    public sealed class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<List<Customer>> SearchAsync(string searchTerm, params string[] searchFields)
        {
            return await _dbSet
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && 
                           (e.Name.Contains(searchTerm) || 
                            e.ContactPerson.Contains(searchTerm) || 
                            (e.Email != null && e.Email.Contains(searchTerm))))
                .ToListAsync();
        }

        public async Task<List<Customer>> GetActiveCustomersAsync()
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

        public async Task<bool> ExistsByTaxNumberAsync(string taxNumber, long excludeId = 0)
        {
            return await _dbSet
                .Where(e => EF.Property<bool>(e, "IsDeleted") == false && e.TaxNumber == taxNumber && e.Id != excludeId)
                .AnyAsync();
        }
    }
} 