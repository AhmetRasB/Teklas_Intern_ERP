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

        // Bill of Material Error Messages
        public const string BOMCodeRequired = "Reçete kodu zorunludur.";
        public const string BOMCodeMaxLength = "Reçete kodu en fazla 50 karakter olabilir.";
        public const string BOMNameRequired = "Reçete adı zorunludur.";
        public const string BOMNameMaxLength = "Reçete adı en fazla 200 karakter olabilir.";
        public const string ProductMaterialCardRequired = "Ürün malzeme kartı seçimi zorunludur.";
        public const string BOMVersionMaxLength = "Reçete versiyon en fazla 20 karakter olabilir.";
        public const string BaseQuantityMustBePositive = "Temel miktar pozitif olmalıdır.";
        public const string UnitRequired = "Birim zorunludur.";
        public const string UnitMaxLength = "Birim en fazla 10 karakter olabilir.";
        public const string BOMTypeRequired = "Reçete tipi zorunludur.";
        public const string InvalidBOMType = "Geçerli bir reçete tipi seçiniz. (PRODUCTION, ASSEMBLY, DISASSEMBLY, PACKAGING)";
        public const string EffectiveToMustBeAfterEffectiveFrom = "Bitiş tarihi başlangıç tarihinden sonra olmalıdır.";
        public const string RouteCodeMaxLength = "Rota kodu en fazla 100 karakter olabilir.";
        public const string StandardTimeMustBeNonNegative = "Standart süre negatif olamaz.";
        public const string SetupTimeMustBeNonNegative = "Hazırlık süresi negatif olamaz.";
        public const string ApprovalStatusRequired = "Onay durumu zorunludur.";
        public const string InvalidApprovalStatus = "Geçerli bir onay durumu seçiniz. (DRAFT, APPROVED, OBSOLETE, PENDING)";
        public const string BOMItemsRequired = "Onaylanmış reçeteler için en az bir kalem gereklidir.";

        // Bill of Material Item Error Messages
        public const string LineNumberMustBePositive = "Satır numarası pozitif olmalıdır.";
        public const string QuantityMustBePositive = "Miktar pozitif olmalıdır.";
        public const string ScrapFactorMustBeNonNegative = "Fire oranı negatif olamaz.";
        public const string ScrapFactorMaxValue = "Fire oranı 100'den büyük olamaz.";
        public const string ComponentTypeRequired = "Komponent tipi zorunludur.";
        public const string InvalidComponentType = "Geçerli bir komponent tipi seçiniz. (RAW_MATERIAL, SEMI_FINISHED, FINISHED_GOOD, PACKAGING, CONSUMABLE)";
        public const string IssueMethodRequired = "Çıkış yöntemi zorunludur.";
        public const string InvalidIssueMethod = "Geçerli bir çıkış yöntemi seçiniz. (MANUAL, AUTOMATIC, BACKFLUSH)";
        public const string OperationSequenceMustBePositive = "Operasyon sırası pozitif olmalıdır.";
        public const string ValidToMustBeAfterValidFrom = "Bitiş tarihi başlangıç tarihinden sonra olmalıdır.";
        public const string CostAllocationMustBeNonNegative = "Maliyet tahsisi negatif olamaz.";
        public const string CostAllocationMaxValue = "Maliyet tahsisi 100'den büyük olamaz.";
        public const string LeadTimeOffsetMustBeNonNegative = "Teslim süresi farkı negatif olamaz.";

        // Work Order Error Messages
        public const string WorkOrderNumberRequired = "İş emri numarası zorunludur.";
        public const string WorkOrderNumberMaxLength = "İş emri numarası en fazla 50 karakter olabilir.";
        public const string BillOfMaterialRequired = "Reçete seçimi zorunludur.";
        public const string PlannedQuantityMustBePositive = "Planlanan miktar pozitif olmalıdır.";
        public const string CompletedQuantityMustBeNonNegative = "Tamamlanan miktar negatif olamaz.";
        public const string CompletedQuantityCannotExceedPlanned = "Tamamlanan miktar planlanan miktarı aşamaz.";
        public const string ScrapQuantityMustBeNonNegative = "Hurda miktar negatif olamaz.";
        public const string StatusRequired = "Durum zorunludur.";
        public const string InvalidWorkOrderStatus = "Geçerli bir iş emri durumu seçiniz. (CREATED, RELEASED, IN_PROGRESS, COMPLETED, CANCELLED, ON_HOLD)";
        public const string PriorityMustBeBetween1And5 = "Öncelik 1-5 arasında olmalıdır.";
        public const string PlannedEndDateMustBeAfterStartDate = "Planlanan bitiş tarihi başlangıç tarihinden sonra olmalıdır.";
        public const string ActualEndDateMustBeAfterStartDate = "Gerçekleşen bitiş tarihi başlangıç tarihinden sonra olmalıdır.";
        public const string DueDateMustBeAfterPlannedStartDate = "Teslim tarihi planlanan başlangıç tarihinden sonra olmalıdır.";
        public const string CustomerOrderReferenceMaxLength = "Müşteri sipariş referansı en fazla 100 karakter olabilir.";
        public const string WorkCenterMaxLength = "İş merkezi en fazla 100 karakter olabilir.";
        public const string ShiftMaxLength = "Vardiya en fazla 50 karakter olabilir.";
        public const string PlannedSetupTimeMustBeNonNegative = "Planlanan hazırlık süresi negatif olamaz.";
        public const string PlannedRunTimeMustBeNonNegative = "Planlanan çalışma süresi negatif olamaz.";
        public const string ActualSetupTimeMustBeNonNegative = "Gerçekleşen hazırlık süresi negatif olamaz.";
        public const string ActualRunTimeMustBeNonNegative = "Gerçekleşen çalışma süresi negatif olamaz.";
        public const string WorkOrderTypeRequired = "İş emri tipi zorunludur.";
        public const string InvalidWorkOrderType = "Geçerli bir iş emri tipi seçiniz. (PRODUCTION, ASSEMBLY, REWORK, MAINTENANCE, QUALITY_CHECK)";
        public const string InvalidSourceType = "Geçerli bir kaynak tipi seçiniz. (MANUAL, SALES_ORDER, FORECAST, STOCK_REPLENISHMENT, PLANNING)";
        public const string CompletionPercentageMustBeBetween0And100 = "Tamamlanma yüzdesi 0-100 arasında olmalıdır.";
        public const string QualityStatusRequired = "Kalite durumu zorunludur.";
        public const string InvalidQualityStatus = "Geçerli bir kalite durumu seçiniz. (NOT_REQUIRED, PENDING, PASSED, FAILED, IN_PROGRESS)";
        public const string ActualStartDateRequiredForInProgressOrCompleted = "Devam eden veya tamamlanan iş emirleri için gerçek başlangıç tarihi gereklidir.";
        public const string ActualEndDateRequiredForCompleted = "Tamamlanan iş emirleri için gerçek bitiş tarihi gereklidir.";
        public const string TotalCompletedCannotExceedPlanned = "Toplam tamamlanan miktar planlanan miktarı aşamaz.";

        // Production Confirmation Error Messages
        public const string WorkOrderRequired = "İş emri seçimi zorunludur.";
        public const string ConfirmationNumberRequired = "Onay numarası zorunludur.";
        public const string ConfirmationNumberMaxLength = "Onay numarası en fazla 50 karakter olabilir.";
        public const string ConfirmationDateRequired = "Onay tarihi zorunludur.";
        public const string ConfirmationDateCannotBeFuture = "Onay tarihi gelecek tarih olamaz.";
        public const string ConfirmedQuantityMustBeNonNegative = "Onaylanan miktar negatif olamaz.";
        public const string ReworkQuantityMustBeNonNegative = "Yeniden işleme miktarı negatif olamaz.";
        public const string InvalidConfirmationStatus = "Geçerli bir onay durumu seçiniz. (DRAFT, CONFIRMED, CANCELLED, POSTED)";
        public const string ConfirmationTypeRequired = "Onay tipi zorunludur.";
        public const string InvalidConfirmationType = "Geçerli bir onay tipi seçiniz. (GOOD, SCRAP, REWORK, SETUP, MAINTENANCE, QUALITY_CHECK)";
        public const string RunTimeMustBeNonNegative = "Çalışma süresi negatif olamaz.";
        public const string DownTimeMustBeNonNegative = "Duruş süresi negatif olamaz.";
        public const string DownTimeReasonRequiredWhenDownTimeExists = "Duruş süresi girildiğinde duruş nedeni zorunludur.";
        public const string DownTimeReasonMaxLength = "Duruş nedeni en fazla 200 karakter olabilir.";
        public const string NotesMaxLength = "Notlar en fazla 500 karakter olabilir.";
        public const string QualityNotesMaxLength = "Kalite notları en fazla 500 karakter olabilir.";
        public const string SerialNumberToMustBeGreaterThanFrom = "Bitiş seri numarası başlangıç seri numarasından büyük olmalıdır.";
        public const string CostCenterMaxLength = "Masraf merkezi en fazla 50 karakter olabilir.";
        public const string TotalQuantityMustBeGreaterThanZero = "Toplam miktar sıfırdan büyük olmalıdır.";
        public const string OperatorRequiredForConfirmedProduction = "Onaylanmış üretim için operatör seçimi zorunludur.";
        public const string ConfirmedByUserRequiredForConfirmedProduction = "Onaylanmış üretim için onaylayan kullanıcı seçimi zorunludur.";
        public const string ConfirmedDateRequiredForConfirmedProduction = "Onaylanmış üretim için onay tarihi zorunludur.";
        public const string QualityNotesRequiredForFailedQuality = "Başarısız kalite kontrolü için kalite notları zorunludur.";
    }
} 