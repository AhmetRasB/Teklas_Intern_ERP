namespace Teklas_Intern_ERP.DTOs
{
    public static class Error
    {
        // Material Card Error Messages
        public const string MaterialNameRequired = "Malzeme adı zorunludur.";
        public const string MaterialCodeRequired = "Malzeme kodu zorunludur.";
        public const string MaterialNameMinLength = "Malzeme adı en az 2 karakter olmalıdır.";
        public const string MaterialCodeMinLength = "Malzeme kodu en az 3 karakter olmalıdır.";
        public const string MaterialNameMaxLength = "Malzeme adı en fazla 100 karakter olabilir.";
        public const string MaterialCodeMaxLength = "Malzeme kodu en fazla 20 karakter olabilir.";
        public const string MaterialTypeRequired = "Malzeme tipi zorunludur.";
        public const string CategoryIdRequired = "Kategori seçimi zorunludur.";
        public const string UnitOfMeasureRequired = "Birim ölçüsü zorunludur.";
        public const string PurchasePricePositive = "Alış fiyatı pozitif olmalıdır.";
        public const string SalesPricePositive = "Satış fiyatı pozitif olmalıdır.";
        public const string MinimumStockLevelPositive = "Minimum stok seviyesi pozitif olmalıdır.";
        public const string MaximumStockLevelPositive = "Maksimum stok seviyesi pozitif olmalıdır.";
        public const string ReorderLevelPositive = "Yeniden sipariş seviyesi pozitif olmalıdır.";
        public const string ShelfLifePositive = "Raf ömrü pozitif olmalıdır.";
        public const string WeightPositive = "Ağırlık pozitif olmalıdır.";
        public const string VolumePositive = "Hacim pozitif olmalıdır.";
        public const string LengthPositive = "Uzunluk pozitif olmalıdır.";
        public const string WidthPositive = "Genişlik pozitif olmalıdır.";
        public const string HeightPositive = "Yükseklik pozitif olmalıdır.";
        public const string BarcodeMaxLength = "Barkod en fazla 50 karakter olabilir.";
        public const string DescriptionMaxLength = "Açıklama en fazla 500 karakter olabilir.";
        public const string BrandMaxLength = "Marka en fazla 50 karakter olabilir.";
        public const string ModelMaxLength = "Model en fazla 50 karakter olabilir.";
        public const string ColorMaxLength = "Renk en fazla 30 karakter olabilir.";
        public const string OriginCountryMaxLength = "Menşei ülke en fazla 50 karakter olabilir.";
        public const string ManufacturerMaxLength = "Üretici en fazla 100 karakter olabilir.";
        
        // Generic Error Messages
        public const string FieldRequired = "Bu alan zorunludur.";
        public const string FieldMinLength = "Bu alan en az {0} karakter olmalıdır.";
        public const string FieldMaxLength = "Bu alan en fazla {0} karakter olabilir.";
        public const string FieldPositive = "Bu alan pozitif olmalıdır.";
        public const string FieldEmail = "Geçerli bir e-posta adresi giriniz.";
        public const string FieldPhone = "Geçerli bir telefon numarası giriniz.";
        public const string FieldDate = "Geçerli bir tarih giriniz.";
        public const string FieldNumeric = "Bu alan sayısal olmalıdır.";
    }
} 