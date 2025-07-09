-- MaterialCategory Test Data (Turkce karakterler ingilizceye cevrildi)
INSERT INTO MaterialCategories (Code, Name, Description, ParentCategoryId, Status, IsDeleted, CreateDate, UpdateDate, CreateUserId, UpdateUserId)
VALUES
('HAM', 'Hammadde', 'Hammadde', NULL, 1, 0, GETDATE(), GETDATE(), 1, 1),
('YM', 'Yari Mamul', 'Yari Mamul', NULL, 1, 0, GETDATE(), GETDATE(), 1, 1),
('MAM', 'Mamul', 'Mamul', NULL, 1, 0, GETDATE(), GETDATE(), 1, 1),
('YARD', 'Yardimci Malzeme', 'Yardimci Malzeme', NULL, 1, 0, GETDATE(), GETDATE(), 1, 1),
('ELEK', 'Elektronik', 'Elektronik', NULL, 1, 0, GETDATE(), GETDATE(), 1, 1);

-- MaterialCard Test Data (Turkce karakterler ingilizceye cevrildi)
INSERT INTO MaterialCards (
    MaterialCode, 
    MaterialName, 
    MaterialType, 
    CategoryId, 
    UnitOfMeasure, 
    Barcode, 
    Description, 
    Brand, 
    Model, 
    PurchasePrice, 
    SalesPrice, 
    MinimumStockLevel, 
    MaximumStockLevel, 
    ReorderLevel, 
    ShelfLife, 
    Weight, 
    Volume, 
    Length, 
    Width, 
    Height, 
    Color, 
    OriginCountry, 
    Manufacturer, 
    ManufacturerPartNumber,
    Status, 
    IsDeleted, 
    CreatedDate, 
    UpdatedDate, 
    CreateUserId, 
    UpdateUserId,
    CreateDate,
    UpdateDate
) VALUES 
('MC001', 'Celik Levha 2mm', 'Hammadde', 1, 'Adet', '1234567890123', '2mm kalinliginda celik levha', 'ArcelorMittal', 'ST-2MM-1000x2000', 150.00, 180.00, 50, 200, 75, NULL, 15.7, 0.004, 2000, 1000, 2, 'Gri', 'Turkiye', 'ArcelorMittal', 'ST-2MM-1000x2000', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),
('MC002', 'Aluminyum Profil 6063', 'Hammadde', 1, 'Metre', '1234567890124', '6063 aluminyum profil', 'Hydro', 'AL-6063-40x40', 25.50, 32.00, 100, 500, 150, NULL, 2.1, 0.0016, 6000, 40, 40, 'Gumus', 'Turkiye', 'Hydro', 'AL-6063-40x40', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),
('MC003', 'Motor Govdesi', 'Yari Mamul', 2, 'Adet', '1234567890125', 'Dizel motor govdesi 2.0L', 'Teklas', 'MG-2.0L-DIESEL', 850.00, 1200.00, 20, 100, 30, NULL, 45.5, 0.045, 450, 300, 350, 'Siyah', 'Turkiye', 'Teklas', 'MG-2.0L-DIESEL', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),
('MC004', 'Fren Sistemi Montaji', 'Yari Mamul', 2, 'Adet', '1234567890126', 'On fren sistemi tam montaj', 'Brembo', 'FS-FRONT-COMPLETE', 320.00, 450.00, 15, 80, 25, NULL, 8.2, 0.008, 400, 200, 150, 'Kirmizi', 'Italya', 'Brembo', 'FS-FRONT-COMPLETE', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),
('MC005', 'Otomobil Motoru 2.0L', 'Mamul', 3, 'Adet', '1234567890127', 'Tamamlanmis 2.0L dizel motor', 'Teklas', 'ENG-2.0L-COMPLETE', 2500.00, 3500.00, 5, 25, 8, NULL, 180.0, 0.180, 600, 400, 500, 'Siyah', 'Turkiye', 'Teklas', 'ENG-2.0L-COMPLETE', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()); 