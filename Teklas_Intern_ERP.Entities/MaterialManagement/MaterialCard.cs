using System;

namespace Teklas_Intern_ERP.Entities.MaterialManagement
{
    public class MaterialCard
    {
        public int Id { get; set; } // Birincil anahtar
        public string MaterialCode { get; set; } // Malzeme kodu
        public string MaterialName { get; set; } // Malzeme adı
        public string MaterialType { get; set; } // Hammadde, yarı mamul, mamul, sarf malzeme
        public int CategoryId { get; set; } // Kategori FK
        public string UnitOfMeasure { get; set; } // Ölçü birimi
        public string Barcode { get; set; } // Barkod
        public string Description { get; set; } // Açıklama
        public string Brand { get; set; } // Marka
        public string Model { get; set; } // Model
        public decimal PurchasePrice { get; set; } // Alış fiyatı
        public decimal SalesPrice { get; set; } // Satış fiyatı
        public decimal MinimumStockLevel { get; set; } // Minimum stok
        public decimal MaximumStockLevel { get; set; } // Maksimum stok
        public decimal ReorderLevel { get; set; } // Yeniden sipariş seviyesi
        public int? ShelfLife { get; set; } // Raf ömrü (gün)
        public decimal? Weight { get; set; } // Ağırlık (kg)
        public decimal? Volume { get; set; } // Hacim (m³)
        public decimal? Length { get; set; } // Uzunluk (cm)
        public decimal? Width { get; set; } // Genişlik (cm)
        public decimal? Height { get; set; } // Yükseklik (cm)
        public string Color { get; set; } // Renk
        public string OriginCountry { get; set; } // Menşei ülke
        public string Manufacturer { get; set; } // Üretici
        public bool IsActive { get; set; } // Aktif/pasif
        public DateTime CreatedDate { get; set; } // Oluşturulma tarihi
        public DateTime? UpdatedDate { get; set; } // Son güncellenme tarihi
        public string CreatedBy { get; set; } // Kaydı oluşturan kullanıcı
        public string UpdatedBy { get; set; } // Son güncelleyen kullanıcı
    }
} 