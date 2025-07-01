using System;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    public class MaterialCategory
    {
        public int Id { get; set; } // Birincil anahtar
        public string CategoryCode { get; set; } // Kategori kodu
        public string CategoryName { get; set; } // Kategori adı
        public string Description { get; set; } // Açıklama
        public int? ParentCategoryId { get; set; } // Üst kategori (hiyerarşi için)
        public bool IsActive { get; set; } // Aktif/pasif
        public DateTime CreatedDate { get; set; } // Oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // Son güncellenme tarihi
    }
} 