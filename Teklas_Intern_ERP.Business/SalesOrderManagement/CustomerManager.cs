using System.Collections.Generic;
using System.Linq;
using Teklas_Intern_ERP.Entities.SalesOrderManagement;
using Teklas_Intern_ERP.DataAccess;

namespace Teklas_Intern_ERP.Business.SalesOrderManagement
{
    public class CustomerManager
    {
        private readonly AppDbContext _context;
        public CustomerManager(AppDbContext context)
        {
            _context = context;
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
    }
} 