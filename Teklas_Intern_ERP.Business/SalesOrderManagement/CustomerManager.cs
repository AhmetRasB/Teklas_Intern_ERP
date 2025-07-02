using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.SalesOrderManagement;
using Teklas_Intern_ERP.DataAccess;
using Teklas_Intern_ERP.DataAccess.SalesOrderManagement;
using System.Threading.Tasks;

namespace Teklas_Intern_ERP.Business.SalesOrderManagement
{
    public class CustomerManager
    {
        private readonly AppDbContext _context;
        private readonly CustomerRepository _repo;
        public CustomerManager(AppDbContext context, CustomerRepository repo)
        {
            _context = context;
            _repo = repo;
        }

        public List<Customer> GetAll() => _context.Customers.ToList();
        public Customer GetById(int id) => _context.Customers.Find(id);
        public Customer Add(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();
            return customer;
        }
        public bool Update(Customer customer)
        {
            var existing = _context.Customers.Find(customer.Id);
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
            _context.SaveChanges();
            return true;
        }
        public bool Delete(int id)
        {
            var customer = _context.Customers.Find(id);
            if (customer == null) return false;
            _context.Customers.Remove(customer);
            _context.SaveChanges();
            return true;
        }
        public async Task<List<Customer>> GetAllAsync() => await _repo.GetAllAsync();
        public async Task<Customer> GetByIdAsync(int id) => await _repo.GetByIdAsync(id);
        public async Task<Customer> AddAsync(Customer customer) => await _repo.AddAsync(customer);
        public async Task<bool> UpdateAsync(Customer customer) => await _repo.UpdateAsync(customer);
        public async Task<bool> DeleteAsync(int id) => await _repo.DeleteAsync(id);
    }
} 