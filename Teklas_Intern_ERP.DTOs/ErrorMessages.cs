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
        
        // Material Category Error Messages
        public const string CategoryCodeRequired = "Kategori kodu zorunludur.";
        public const string CategoryNameRequired = "Kategori adı zorunludur.";
        public const string CategoryCodeMaxLength = "Kategori kodu en fazla 50 karakter olabilir.";
        public const string CategoryNameMaxLength = "Kategori adı en fazla 100 karakter olabilir.";
        public const string CategoryDescriptionMaxLength = "Kategori açıklaması en fazla 500 karakter olabilir.";
        
        // Generic Error Messages
        public const string FieldRequired = "Bu alan zorunludur.";
        public const string FieldMinLength = "Bu alan en az {0} karakter olmalıdır.";
        public const string FieldMaxLength = "Bu alan en fazla {0} karakter olabilir.";
        public const string FieldPositive = "Bu alan pozitif olmalıdır.";
        public const string FieldEmail = "Geçerli bir e-posta adresi giriniz.";
        public const string FieldPhone = "Geçerli bir telefon numarası giriniz.";
        public const string FieldDate = "Geçerli bir tarih giriniz.";
        public const string FieldNumeric = "Bu alan sayısal olmalıdır.";

        // User Error Messages
        public const string UsernameRequired = "Kullanıcı adı zorunludur.";
        public const string UsernameMinLength = "Kullanıcı adı en az 3 karakter olmalıdır.";
        public const string UsernameMaxLength = "Kullanıcı adı en fazla 50 karakter olabilir.";
        public const string UsernameInvalidChars = "Kullanıcı adı sadece harf, rakam ve alt çizgi içerebilir.";
        public const string EmailRequired = "E-posta adresi zorunludur.";
        public const string EmailInvalid = "Geçerli bir e-posta adresi giriniz.";
        public const string EmailMaxLength = "E-posta adresi en fazla 100 karakter olabilir.";
        public const string FirstNameRequired = "Ad zorunludur.";
        public const string FirstNameMaxLength = "Ad en fazla 50 karakter olabilir.";
        public const string LastNameRequired = "Soyad zorunludur.";
        public const string LastNameMaxLength = "Soyad en fazla 50 karakter olabilir.";

        // Authentication Error Messages
        public const string UsernameOrEmailRequired = "Kullanıcı adı veya e-posta adresi zorunludur.";
        public const string PasswordRequired = "Şifre zorunludur.";
        public const string PasswordMinLength = "Şifre en az 6 karakter olmalıdır.";
        public const string PasswordComplexity = "Şifre en az bir küçük harf, bir büyük harf ve bir rakam içermelidir.";
        public const string ConfirmPasswordRequired = "Şifre tekrarı zorunludur.";
        public const string PasswordsNotMatch = "Şifreler eşleşmiyor.";

        // Role Error Messages
        public const string RoleNameRequired = "Rol adı zorunludur.";
        public const string RoleNameMinLength = "Rol adı en az 2 karakter olmalıdır.";
        public const string RoleNameMaxLength = "Rol adı en fazla 50 karakter olabilir.";
        public const string RoleDescriptionMaxLength = "Rol açıklaması en fazla 500 karakter olabilir.";
        public const string RolePriorityPositive = "Rol önceliği pozitif olmalıdır.";

        // Material Movement Error Messages
        public const string MaterialCardRequired = "Malzeme kartı seçimi zorunludur.";
        public const string MaterialCardIdInvalid = "Geçerli bir malzeme kartı seçiniz.";
        public const string MovementTypeRequired = "Hareket tipi zorunludur.";
        public const string MovementTypeMaxLength = "Hareket tipi en fazla 50 karakter olabilir.";
        public const string MovementTypeInvalid = "Geçerli bir hareket tipi seçiniz. (IN, OUT, TRANSFER, ADJUSTMENT, PRODUCTION, CONSUMPTION, RETURN)";
        public const string QuantityRequired = "Miktar zorunludur.";
        public const string QuantityCannotBeZero = "Miktar sıfır olamaz.";
        public const string UnitPricePositive = "Birim fiyat pozitif olmalıdır.";
        public const string MovementDateRequired = "Hareket tarihi zorunludur.";
        public const string MovementDateFuture = "Hareket tarihi gelecek tarih olamaz.";
        public const string ReferenceNumberMaxLength = "Referans numarası en fazla 100 karakter olabilir.";
        public const string ReferenceTypeMaxLength = "Referans tipi en fazla 50 karakter olabilir.";
        public const string LocationFromMaxLength = "Çıkış lokasyonu en fazla 100 karakter olabilir.";
        public const string LocationToMaxLength = "Varış lokasyonu en fazla 100 karakter olabilir.";
        public const string ResponsiblePersonMaxLength = "Sorumlu kişi en fazla 100 karakter olabilir.";
        public const string SupplierCustomerMaxLength = "Tedarikçi/Müşteri en fazla 100 karakter olabilir.";
        public const string BatchNumberMaxLength = "Parti numarası en fazla 50 karakter olabilir.";
        public const string SerialNumberMaxLength = "Seri numarası en fazla 50 karakter olabilir.";
        public const string ExpiryDatePast = "Son kullanma tarihi geçmiş tarih olamaz.";
        public const string StatusMaxLength = "Durum en fazla 50 karakter olabilir.";
        public const string StatusInvalid = "Geçerli bir durum seçiniz. (PENDING, CONFIRMED, CANCELLED, COMPLETED)";
        public const string BOMHeaderIdRequired = "BOM (Ürün Ağacı) seçimi zorunludur.";
        public const string BOMItemIdRequired = "BOM kalemi seçimi zorunludur.";
        public const string WorkOrderIdRequired = "İş emri seçimi zorunludur.";
        public const string ConfirmationIdRequired = "Üretim teyit kaydı seçimi zorunludur.";
        public const string MaterialCardIdRequired = "Malzeme kartı seçimi zorunludur.";
        public const string VersionMaxLength = "Versiyon en fazla 20 karakter olabilir.";
        public const string NotesMaxLength = "Açıklama en fazla 1000 karakter olabilir.";
        public const string QuantityMustBePositive = "Miktar pozitif olmalıdır.";
        public const string QuantityMustBeNonNegative = "Miktar negatif olamaz.";
        public const string ScrapRateMustBeNonNegative = "Fire oranı negatif olamaz.";
        public const string LaborHoursMustBeNonNegative = "İşçilik saati negatif olamaz.";
        public const string PerformedByMaxLength = "Yapan kişi en fazla 100 karakter olabilir.";
        public const string ConfirmationDateRequired = "Teyit tarihi zorunludur.";
        public const string PlannedStartDateRequired = "Planlanan başlama tarihi zorunludur.";
        public const string OperationIdRequired = "Operasyon seçimi zorunludur.";
        public const string OperationNameRequired = "Operasyon adı zorunludur.";
        public const string OperationNameMaxLength = "Operasyon adı en fazla 100 karakter olabilir.";
        public const string SequenceMustBePositive = "Operasyon sırası pozitif olmalıdır.";
        public const string PlannedHoursMustBeNonNegative = "Planlanan saat negatif olamaz.";
        public const string ResourceMaxLength = "Kaynak en fazla 100 karakter olabilir.";
    }
} 