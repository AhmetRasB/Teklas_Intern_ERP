using System;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    public class MaterialBatch
    {
        public int Id { get; set; } // Birincil anahtar
        public int MaterialId { get; set; } // Malzeme FK
        public string BatchNo { get; set; } // Parti/Lot numarası
        public DateTime? ExpiryDate { get; set; } // Son kullanma tarihi
        public decimal Quantity { get; set; } // Miktar
        public DateTime CreatedDate { get; set; } // Oluşturulma tarihi
    }
} 