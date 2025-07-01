using System;

namespace Teklas_Intern_ERP.Entities.SalesOrderManagement
{
    public class Customer
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerName { get; set; }
        public string TaxNumber { get; set; }
        public string TaxOffice { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string ContactPerson { get; set; }
        public string IBAN { get; set; }
        public string BankName { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
} 