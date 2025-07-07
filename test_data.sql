-- MaterialCard Test Data
-- Teklas ERP Material Management Test Data

USE TeklasERP;

-- Test verilerini eklemeden önce tabloyu temizle (isteğe bağlı)
-- DELETE FROM MaterialCards;

-- Test MaterialCard verileri
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
    IsActive, 
    IsDeleted, 
    CreatedDate, 
    UpdatedDate, 
    CreatedBy, 
    UpdatedBy
) VALUES 
-- Hammadde Kategorisi
('MC001', 'Çelik Levha 2mm', 'Hammadde', 1, 'Adet', '1234567890123', '2mm kalınlığında çelik levha, otomotiv sektörü için', 'ArcelorMittal', 'ST-2MM-1000x2000', 150.00, 180.00, 50, 200, 75, NULL, 15.7, 0.004, 2000, 1000, 2, 'Gri', 'Türkiye', 'ArcelorMittal', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC002', 'Alüminyum Profil 6063', 'Hammadde', 1, 'Metre', '1234567890124', '6063 alüminyum profil, pencere sistemleri için', 'Hydro', 'AL-6063-40x40', 25.50, 32.00, 100, 500, 150, NULL, 2.1, 0.0016, 6000, 40, 40, 'Gümüş', 'Türkiye', 'Hydro', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC003', 'Plastik Granül PP', 'Hammadde', 1, 'Kilogram', '1234567890125', 'Polypropylene granül, enjeksiyon kalıplama için', 'Petkim', 'PP-GRANUL-001', 12.75, 16.50, 500, 2000, 750, 24, 0.95, 0.001, NULL, NULL, NULL, 'Beyaz', 'Türkiye', 'Petkim', 1, 0, GETDATE(), NULL, 'System', 'System'),

-- Yarı Mamul Kategorisi
('MC004', 'Motor Gövdesi', 'Yarı Mamul', 2, 'Adet', '1234567890126', 'Dizel motor gövdesi, 2.0L hacim', 'Teklas', 'MG-2.0L-DIESEL', 850.00, 1200.00, 20, 100, 30, NULL, 45.5, 0.045, 450, 300, 350, 'Siyah', 'Türkiye', 'Teklas', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC005', 'Fren Sistemi Montajı', 'Yarı Mamul', 2, 'Adet', '1234567890127', 'Ön fren sistemi tam montaj', 'Brembo', 'FS-FRONT-COMPLETE', 320.00, 450.00, 15, 80, 25, NULL, 8.2, 0.008, 400, 200, 150, 'Kırmızı', 'İtalya', 'Brembo', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC006', 'Elektronik Kontrol Ünitesi', 'Yarı Mamul', 2, 'Adet', '1234567890128', 'Motor kontrol ünitesi, CAN bus destekli', 'Bosch', 'ECU-MOTOR-CAN', 180.00, 280.00, 10, 50, 15, NULL, 0.8, 0.0005, 120, 80, 25, 'Yeşil', 'Almanya', 'Bosch', 1, 0, GETDATE(), NULL, 'System', 'System'),

-- Mamul Kategorisi
('MC007', 'Otomobil Motoru 2.0L', 'Mamul', 3, 'Adet', '1234567890129', 'Tamamlanmış 2.0L dizel motor', 'Teklas', 'ENG-2.0L-COMPLETE', 2500.00, 3500.00, 5, 25, 8, NULL, 180.0, 0.180, 600, 400, 500, 'Siyah', 'Türkiye', 'Teklas', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC008', 'Araç Klima Sistemi', 'Mamul', 3, 'Adet', '1234567890130', 'Tam otomatik klima sistemi', 'Valeo', 'AC-AUTO-COMPLETE', 420.00, 650.00, 8, 40, 12, NULL, 12.5, 0.012, 800, 400, 200, 'Beyaz', 'Fransa', 'Valeo', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC009', 'Güvenlik Kemeri Sistemi', 'Mamul', 3, 'Adet', '1234567890131', 'Ön koltuk güvenlik kemeri sistemi', 'Autoliv', 'SB-FRONT-SYSTEM', 85.00, 130.00, 25, 120, 35, NULL, 2.8, 0.003, 300, 150, 100, 'Siyah', 'İsveç', 'Autoliv', 1, 0, GETDATE(), NULL, 'System', 'System'),

-- Yardımcı Malzeme Kategorisi
('MC010', 'Motor Yağı 5W-30', 'Yardımcı Malzeme', 4, 'Litre', '1234567890132', 'Sentetik motor yağı, 5W-30 viskozite', 'Castrol', 'OIL-5W30-SYNTH', 45.00, 65.00, 100, 500, 150, 36, 0.85, 0.001, NULL, NULL, NULL, 'Altın', 'İngiltere', 'Castrol', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC011', 'Fren Hidrolik Yağı', 'Yardımcı Malzeme', 4, 'Litre', '1234567890133', 'DOT 4 fren hidrolik yağı', 'Shell', 'BRAKE-DOT4', 18.50, 28.00, 50, 200, 75, 24, 1.05, 0.001, NULL, NULL, NULL, 'Şeffaf', 'Hollanda', 'Shell', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC012', 'Soğutma Sıvısı', 'Yardımcı Malzeme', 4, 'Litre', '1234567890134', 'Antifriz soğutma sıvısı, -40°C', 'Prestone', 'COOLANT-AF40', 22.00, 35.00, 80, 300, 120, 60, 1.08, 0.001, NULL, NULL, NULL, 'Yeşil', 'ABD', 'Prestone', 1, 0, GETDATE(), NULL, 'System', 'System'),

-- Elektronik Kategorisi
('MC013', 'Sensör Oksijen', 'Elektronik', 5, 'Adet', '1234567890135', 'Lambda sensörü, oksijen seviyesi ölçümü', 'NGK', 'SENSOR-O2-LAMBDA', 95.00, 145.00, 15, 60, 20, NULL, 0.15, 0.0001, 80, 25, 25, 'Siyah', 'Japonya', 'NGK', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC014', 'Ateşleme Bobini', 'Elektronik', 5, 'Adet', '1234567890136', 'Yüksek performanslı ateşleme bobini', 'Denso', 'IGNITION-COIL-HP', 65.00, 95.00, 20, 80, 25, NULL, 0.3, 0.0002, 120, 45, 35, 'Siyah', 'Japonya', 'Denso', 1, 0, GETDATE(), NULL, 'System', 'System'),

('MC015', 'Radyo Anteni', 'Elektronik', 5, 'Adet', '1234567890137', 'Çekmeli radyo anteni, 1.2m uzunluk', 'Pioneer', 'ANTENNA-TELESCOPIC', 28.00, 42.00, 30, 100, 40, NULL, 0.2, 0.0001, 1200, 8, 8, 'Siyah', 'Japonya', 'Pioneer', 1, 0, GETDATE(), NULL, 'System', 'System');

-- Verilerin başarıyla eklendiğini kontrol et
SELECT 
    MaterialCode,
    MaterialName,
    MaterialType,
    PurchasePrice,
    SalesPrice,
    IsActive
FROM MaterialCards 
ORDER BY MaterialCode; 