using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.SalesOrderManagement;

namespace Teklas_Intern_ERP.DataAccess.SalesOrderManagement
{
    public class CustomerRepository
    {
        private readonly AppDbContext _context;
        public CustomerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Customer>> GetAllAsync() => await _context.Customers.ToListAsync();
        public async Task<Customer> GetByIdAsync(int id) => await _context.Customers.FindAsync(id);
        public async Task<Customer> AddAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
            return customer;
        }
        public async Task<bool> UpdateAsync(Customer customer)
        {
            var existing = await _context.Customers.FindAsync(customer.Id);
            if (existing == null) return false;
            existing.CustomerCode = customer.CustomerCode;
            existing.CustomerName = customer.CustomerName;
            existing.TaxNumber = customer.TaxNumber;
            existing.TaxOffice = customer.TaxOffice;
            existing.Address = customer.Address;
            existing.City = customer.City;
            existing.Country = customer.Country;
            existing.Phone = customer.Phone;
            existing.Email = customer.Email;
            existing.ContactPerson = customer.ContactPerson;
            existing.IBAN = customer.IBAN;
            existing.BankName = customer.BankName;
            existing.IsActive = customer.IsActive;
            existing.CreatedDate = customer.CreatedDate;
            existing.UpdatedDate = customer.UpdatedDate;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 