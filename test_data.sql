-- MaterialCategory Test Data
INSERT INTO MaterialCategories (Code, Name, Status, CreateDate, CreateUserId, IsDeleted)
VALUES
('HAM', 'Hammadde', 1, GETDATE(), 1, 0),
('YM', 'Yarı Mamul', 1, GETDATE(), 1, 0),
('MAM', 'Mamul', 1, GETDATE(), 1, 0),
('YARD', 'Yardımcı Malzeme', 1, GETDATE(), 1, 0),
('ELEK', 'Elektronik', 1, GETDATE(), 1, 0);

-- MaterialCategories Test Data
INSERT INTO MaterialCategories (Code, Name, Description, ParentCategoryId, Status, IsDeleted, CreateDate, UpdateDate) VALUES
('CAT001', 'Elektronik', 'Elektronik malzemeler', NULL, 1, 0, GETDATE(), GETDATE()),
('CAT002', 'Mekanik', 'Mekanik malzemeler', NULL, 1, 0, GETDATE(), GETDATE()),
('CAT003', 'Kimyasal', 'Kimyasal malzemeler', NULL, 1, 0, GETDATE(), GETDATE()),
('CAT004', 'Yarı İletkenler', 'Yarı iletken malzemeler', 1, 1, 0, GETDATE(), GETDATE()),
('CAT005', 'Kondansatörler', 'Kondansatör malzemeleri', 1, 1, 0, GETDATE(), GETDATE()),
('CAT006', 'Dirençler', 'Direnç malzemeleri', 1, 1, 0, GETDATE(), GETDATE()),
('CAT007', 'Vidalar', 'Vida malzemeleri', 2, 1, 0, GETDATE(), GETDATE()),
('CAT008', 'Somunlar', 'Somun malzemeleri', 2, 1, 0, GETDATE(), GETDATE()),
('CAT009', 'Asitler', 'Asit malzemeleri', 3, 1, 0, GETDATE(), GETDATE()),
('CAT010', 'Bazlar', 'Baz malzemeleri', 3, 1, 0, GETDATE(), GETDATE());

-- MaterialCard Test Data - 10 Adet Hızlı Test Verisi
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
('MC001', 'Çelik Levha 2mm', 'Hammadde', 6, 'Adet', '1234567890123', '2mm kalınlığında çelik levha', 'ArcelorMittal', 'ST-2MM-1000x2000', 150.00, 180.00, 50, 200, 75, NULL, 15.7, 0.004, 2000, 1000, 2, 'Gri', 'Türkiye', 'ArcelorMittal', 'ST-2MM-1000x2000', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC002', 'Alüminyum Profil 6063', 'Hammadde', 6, 'Metre', '1234567890124', '6063 alüminyum profil', 'Hydro', 'AL-6063-40x40', 25.50, 32.00, 100, 500, 150, NULL, 2.1, 0.0016, 6000, 40, 40, 'Gümüş', 'Türkiye', 'Hydro', 'AL-6063-40x40', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC003', 'Motor Gövdesi', 'Yarı Mamul', 7, 'Adet', '1234567890125', 'Dizel motor gövdesi 2.0L', 'Teklas', 'MG-2.0L-DIESEL', 850.00, 1200.00, 20, 100, 30, NULL, 45.5, 0.045, 450, 300, 350, 'Siyah', 'Türkiye', 'Teklas', 'MG-2.0L-DIESEL', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC004', 'Fren Sistemi Montajı', 'Yarı Mamul', 7, 'Adet', '1234567890126', 'Ön fren sistemi tam montaj', 'Brembo', 'FS-FRONT-COMPLETE', 320.00, 450.00, 15, 80, 25, NULL, 8.2, 0.008, 400, 200, 150, 'Kırmızı', 'İtalya', 'Brembo', 'FS-FRONT-COMPLETE', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC005', 'Otomobil Motoru 2.0L', 'Mamul', 8, 'Adet', '1234567890127', 'Tamamlanmış 2.0L dizel motor', 'Teklas', 'ENG-2.0L-COMPLETE', 2500.00, 3500.00, 5, 25, 8, NULL, 180.0, 0.180, 600, 400, 500, 'Siyah', 'Türkiye', 'Teklas', 'ENG-2.0L-COMPLETE', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC006', 'Motor Yağı 5W-30', 'Yardımcı Malzeme', 9, 'Litre', '1234567890128', 'Sentetik motor yağı 5W-30', 'Castrol', 'OIL-5W30-SYNTH', 45.00, 65.00, 100, 500, 150, 36, 0.85, 0.001, NULL, NULL, NULL, 'Altın', 'İngiltere', 'Castrol', 'OIL-5W30-SYNTH', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC007', 'Sensör Oksijen', 'Elektronik', 10, 'Adet', '1234567890129', 'Lambda sensörü oksijen ölçümü', 'NGK', 'SENSOR-O2-LAMBDA', 95.00, 145.00, 15, 60, 20, NULL, 0.15, 0.0001, 80, 25, 25, 'Siyah', 'Japonya', 'NGK', 'SENSOR-O2-LAMBDA', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC008', 'Plastik Granül PP', 'Hammadde', 6, 'Kilogram', '1234567890130', 'Polypropylene granül', 'Petkim', 'PP-GRANUL-001', 12.75, 16.50, 500, 2000, 750, 24, 0.95, 0.001, NULL, NULL, NULL, 'Beyaz', 'Türkiye', 'Petkim', 'PP-GRANUL-001', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC009', 'Araç Klima Sistemi', 'Mamul', 8, 'Adet', '1234567890131', 'Tam otomatik klima sistemi', 'Valeo', 'AC-AUTO-COMPLETE', 420.00, 650.00, 8, 40, 12, NULL, 12.5, 0.012, 800, 400, 200, 'Beyaz', 'Fransa', 'Valeo', 'AC-AUTO-COMPLETE', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE()),

('MC010', 'Ateşleme Bobini', 'Elektronik', 10, 'Adet', '1234567890132', 'Yüksek performanslı ateşleme bobini', 'Denso', 'IGNITION-COIL-HP', 65.00, 95.00, 20, 80, 25, NULL, 0.3, 0.0002, 120, 45, 35, 'Siyah', 'Japonya', 'Denso', 'IGNITION-COIL-HP', 1, 0, GETDATE(), GETDATE(), 1, 1, GETDATE(), GETDATE());

-- Material Cards Test Data (Updated with correct column names)
INSERT INTO MaterialCards (MaterialCode, MaterialName, Description, CategoryId, UnitOfMeasure, PurchasePrice, MinimumStockLevel, MaximumStockLevel, ReorderLevel, Status, IsDeleted, CreateDate, UpdateDate) VALUES
('MC001', 'Silikon Çip', 'Yüksek performanslı silikon çip', 4, 'Adet', 15.50, 100, 1000, 500, 1, 0, GETDATE(), GETDATE()),
('MC002', 'Alüminyum Kapasitör', '100uF alüminyum kapasitör', 5, 'Adet', 2.25, 200, 2000, 800, 1, 0, GETDATE(), GETDATE()),
('MC003', 'Karbon Direnç', '10K ohm karbon direnç', 6, 'Adet', 0.15, 500, 5000, 2500, 1, 0, GETDATE(), GETDATE()),
('MC004', 'M6 Vida', 'M6x20 paslanmaz çelik vida', 7, 'Adet', 1.80, 300, 3000, 1200, 1, 0, GETDATE(), GETDATE()),
('MC005', 'M6 Somun', 'M6 paslanmaz çelik somun', 8, 'Adet', 0.90, 300, 3000, 1200, 1, 0, GETDATE(), GETDATE()),
('MC006', 'Sülfürik Asit', '98% sülfürik asit', 9, 'Litre', 25.00, 10, 100, 50, 1, 0, GETDATE(), GETDATE()),
('MC007', 'Sodyum Hidroksit', 'Saf sodyum hidroksit', 10, 'Kg', 12.50, 20, 200, 100, 1, 0, GETDATE(), GETDATE()),
('MC008', 'LED Diod', '5mm kırmızı LED', 4, 'Adet', 0.75, 1000, 10000, 5000, 1, 0, GETDATE(), GETDATE()),
('MC009', 'PCB Board', 'Çift taraflı PCB board', 1, 'Adet', 8.50, 50, 500, 200, 1, 0, GETDATE(), GETDATE()),
('MC010', 'Terminal Bloğu', '10 pin terminal bloğu', 1, 'Adet', 3.25, 100, 1000, 400, 1, 0, GETDATE(), GETDATE());

-- Verilerin başarıyla eklendiğini kontrol et
SELECT 
    MaterialCode,
    MaterialName,
    MaterialType,
    PurchasePrice,
    SalesPrice,
    Status
FROM MaterialCards 
ORDER BY MaterialCode; 