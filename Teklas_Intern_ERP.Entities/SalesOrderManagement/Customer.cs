using System;

namespace Teklas_Intern_ERP.Entities.SalesOrderManagement
{
    public class Customer
    {
        public int Id { get; set; } // Birincil anahtar
        public string CustomerCode { get; set; } // Müşteri kodu
        public string CustomerName { get; set; } // Müşteri adı
        public string TaxNumber { get; set; } // Vergi numarası
        public string TaxOffice { get; set; } // Vergi dairesi
        public string Address { get; set; } // Adres
        public string City { get; set; } // Şehir
        public string Country { get; set; } // Ülke
        public string Phone { get; set; } // Telefon
        public string Email { get; set; } // E-posta
        public string ContactPerson { get; set; } // İrtibat kişisi
        public string IBAN { get; set; } // IBAN
        public string BankName { get; set; } // Banka adı
        public bool IsActive { get; set; } // Aktif/pasif
        public DateTime CreatedDate { get; set; } // Oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // Son güncellenme tarihi
    }
} 