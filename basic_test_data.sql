-- ===================================================================
-- COMPREHENSIVE TEST DATA FOR ALL MODULES (IDENTITY SAFE)
-- ===================================================================

-- 1. Create Material Categories first (only if they don't exist)
IF NOT EXISTS (SELECT 1 FROM MaterialCategories WHERE CategoryCode = 'CAT-001')
BEGIN
    INSERT INTO MaterialCategories (
        CategoryCode, CategoryName, Description, ParentCategoryId,
        Level, DisplayOrder, Status,
        CreateDate, CreateUserId, IsDeleted
    ) VALUES
    ('CAT-001', 'Electronics', 'Electronic products and components', NULL, 1, 1, 1, GETDATE(), 1, 0),
    ('CAT-002', 'Components', 'Raw components for assembly', NULL, 1, 2, 1, GETDATE(), 1, 0),
    ('CAT-003', 'Raw Materials', 'Basic raw materials', NULL, 1, 3, 1, GETDATE(), 1, 0);
END

-- 2. Create Material Cards (only if they don't exist)
IF NOT EXISTS (SELECT 1 FROM MaterialCards WHERE CardCode = 'LAPTOP-001')
BEGIN
    INSERT INTO MaterialCards (
        CardCode, CardName, CardType, Unit, MaterialCategoryId, 
        PurchasePrice, SalesPrice, CurrentStock, Description,
        CreateDate, CreateUserId, IsDeleted, Status
    ) VALUES
    ('LAPTOP-001', 'Gaming Laptop', 'FINISHED_PRODUCT', 'EACH', 1, 800.00, 1200.00, 0, 'Gaming laptop', GETDATE(), 1, 0, 1),
    ('DESKTOP-001', 'Office Desktop', 'FINISHED_PRODUCT', 'EACH', 1, 500.00, 750.00, 0, 'Office desktop computer', GETDATE(), 1, 0, 1),
    ('CPU-001', 'Processor', 'COMPONENT', 'EACH', 2, 200.00, 250.00, 100, 'CPU', GETDATE(), 1, 0, 1),
    ('GPU-001', 'Graphics Card', 'COMPONENT', 'EACH', 2, 300.00, 350.00, 50, 'GPU', GETDATE(), 1, 0, 1),
    ('RAM-001', 'Memory Module', 'COMPONENT', 'EACH', 2, 80.00, 100.00, 200, 'RAM module', GETDATE(), 1, 0, 1),
    ('MB-001', 'Motherboard', 'COMPONENT', 'EACH', 2, 150.00, 180.00, 75, 'Motherboard', GETDATE(), 1, 0, 1),
    ('ALUM-001', 'Aluminum Sheet', 'RAW_MATERIAL', 'KG', 3, 5.00, 6.00, 1000, 'Aluminum sheet', GETDATE(), 1, 0, 1),
    ('PLASTIC-001', 'Plastic Granules', 'RAW_MATERIAL', 'KG', 3, 3.00, 3.50, 2000, 'Plastic granules', GETDATE(), 1, 0, 1);
END

-- 3. Create BOM Headers and capture IDs
DECLARE @LaptopBOMId BIGINT, @DesktopBOMId BIGINT;
INSERT INTO BOMHeaders (
    Id, ParentMaterialCardId, Version, ValidFrom, ValidTo,
    StandardCost, Notes, MaterialCardId, Status,
    CreateDate, CreateUserId, IsDeleted
) VALUES
(1001, 1, 'v1.0', GETDATE(), NULL, 800.00, 'Basic laptop assembly', 1, 1, GETDATE(), 1, 0);
SET @LaptopBOMId = SCOPE_IDENTITY();
INSERT INTO BOMHeaders (
    Id, ParentMaterialCardId, Version, ValidFrom, ValidTo,
    StandardCost, Notes, MaterialCardId, Status,
    CreateDate, CreateUserId, IsDeleted
) VALUES
(1002, 2, 'v1.0', GETDATE(), NULL, 500.00, 'Office desktop assembly', 2, 1, GETDATE(), 1, 0);
SET @DesktopBOMId = SCOPE_IDENTITY();

-- 4. Create BOM Items using captured BOMHeader IDs
INSERT INTO BOMItems (
    BOMHeaderId, ComponentMaterialCardId, Quantity, ScrapRate,
    MaterialCardId, Status, CreateDate, CreateUserId, IsDeleted
) VALUES
-- Laptop BOM Items
(@LaptopBOMId, 3, 1.00, 0.05, 3, 1, GETDATE(), 1, 0),
(@LaptopBOMId, 4, 1.00, 0.03, 4, 1, GETDATE(), 1, 0),
(@LaptopBOMId, 5, 2.00, 0.02, 5, 1, GETDATE(), 1, 0),
(@LaptopBOMId, 6, 1.00, 0.01, 6, 1, GETDATE(), 1, 0),
-- Desktop BOM Items
(@DesktopBOMId, 3, 1.00, 0.05, 3, 1, GETDATE(), 1, 0),
(@DesktopBOMId, 5, 1.00, 0.02, 5, 1, GETDATE(), 1, 0),
(@DesktopBOMId, 6, 1.00, 0.01, 6, 1, GETDATE(), 1, 0);

-- 5. Create Work Orders and capture IDs
DECLARE @WO1Id BIGINT, @WO2Id BIGINT, @WO3Id BIGINT;
INSERT INTO WorkOrders (
    BOMHeaderId, MaterialCardId, PlannedQuantity,
    PlannedStartDate, PlannedEndDate, Status, ReferenceNumber,
    CreateDate, CreateUserId, IsDeleted
) VALUES
(@LaptopBOMId, 1, 10.00, GETDATE(), DATEADD(day, 7, GETDATE()), 1, 'REF-001', GETDATE(), 1, 0);
SET @WO1Id = SCOPE_IDENTITY();
INSERT INTO WorkOrders (
    BOMHeaderId, MaterialCardId, PlannedQuantity,
    PlannedStartDate, PlannedEndDate, Status, ReferenceNumber,
    CreateDate, CreateUserId, IsDeleted
) VALUES
(@DesktopBOMId, 2, 5.00, GETDATE(), DATEADD(day, 5, GETDATE()), 2, 'REF-002', GETDATE(), 1, 0);
SET @WO2Id = SCOPE_IDENTITY();
INSERT INTO WorkOrders (
    BOMHeaderId, MaterialCardId, PlannedQuantity,
    PlannedStartDate, PlannedEndDate, Status, ReferenceNumber,
    CreateDate, CreateUserId, IsDeleted
) VALUES
(@LaptopBOMId, 1, 15.00, DATEADD(day, 1, GETDATE()), DATEADD(day, 10, GETDATE()), 1, 'REF-003', GETDATE(), 1, 0);
SET @WO3Id = SCOPE_IDENTITY();

-- 6. Create Work Order Operations
INSERT INTO WorkOrderOperations (
    WorkOrderId, OperationName, Sequence, PlannedHours, Resource,
    Status, CreateDate, CreateUserId, IsDeleted
) VALUES
(@WO1Id, 'Assembly Preparation', 1, 2.00, 'Assembly Line 1', 1, GETDATE(), 1, 0),
(@WO1Id, 'Component Installation', 2, 4.00, 'Assembly Line 1', 1, GETDATE(), 1, 0),
(@WO1Id, 'Testing', 3, 2.00, 'Test Station 1', 1, GETDATE(), 1, 0),
(@WO2Id, 'Assembly Preparation', 1, 1.50, 'Assembly Line 2', 2, GETDATE(), 1, 0),
(@WO2Id, 'Component Installation', 2, 3.00, 'Assembly Line 2', 1, GETDATE(), 1, 0),
(@WO3Id, 'Assembly Preparation', 1, 3.00, 'Assembly Line 1', 1, GETDATE(), 1, 0),
(@WO3Id, 'Component Installation', 2, 6.00, 'Assembly Line 1', 1, GETDATE(), 1, 0);

-- 7. Create Production Confirmations and capture IDs
DECLARE @PC1Id BIGINT, @PC2Id BIGINT;
INSERT INTO ProductionConfirmations (
    WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped,
    LaborHoursUsed, PerformedBy, MaterialCardId, Status,
    CreateDate, CreateUserId, IsDeleted
) VALUES
(@WO2Id, GETDATE(), 2.00, 0.00, 4.50, 'John Doe', 2, 3, GETDATE(), 1, 0);
SET @PC1Id = SCOPE_IDENTITY();
INSERT INTO ProductionConfirmations (
    WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped,
    LaborHoursUsed, PerformedBy, MaterialCardId, Status,
    CreateDate, CreateUserId, IsDeleted
) VALUES
(@WO2Id, DATEADD(day, 1, GETDATE()), 3.00, 0.00, 6.00, 'Jane Smith', 2, 3, GETDATE(), 1, 0);
SET @PC2Id = SCOPE_IDENTITY();

-- 8. Create Material Consumptions
INSERT INTO MaterialConsumptions (
    ConfirmationId, MaterialCardId, QuantityUsed, UnitPrice, TotalAmount,
    BatchNumber, Description, Status, CreateDate, CreateUserId, IsDeleted
) VALUES
(@PC1Id, 3, 2.00, 200.00, 400.00, 'BATCH-001', 'CPU consumption', 4, GETDATE(), 1, 0),
(@PC1Id, 5, 2.00, 80.00, 160.00, 'BATCH-002', 'RAM consumption', 4, GETDATE(), 1, 0),
(@PC1Id, 6, 2.00, 150.00, 300.00, 'BATCH-003', 'Motherboard consumption', 4, GETDATE(), 1, 0),
(@PC2Id, 3, 3.00, 200.00, 600.00, 'BATCH-004', 'CPU consumption', 4, GETDATE(), 1, 0),
(@PC2Id, 5, 3.00, 80.00, 240.00, 'BATCH-005', 'RAM consumption', 4, GETDATE(), 1, 0),
(@PC2Id, 6, 3.00, 150.00, 450.00, 'BATCH-006', 'Motherboard consumption', 4, GETDATE(), 1, 0);

-- 9. Create Material Movements (for stock tracking)
INSERT INTO MaterialMovements (
    MaterialCardId, MovementType, Quantity, UnitPrice, TotalAmount,
    MovementDate, ReferenceNumber, ReferenceType, LocationFrom, LocationTo,
    Description, ResponsiblePerson, Status, CreateDate, CreateUserId, IsDeleted
) VALUES
(3, 'CONSUMPTION', -5.00, 200.00, -1000.00, GETDATE(), 'WO-002', 'WORK_ORDER', 'WAREHOUSE', 'PRODUCTION', 'CPU consumption for production', 'John Doe', 1, GETDATE(), 1, 0),
(5, 'CONSUMPTION', -5.00, 80.00, -400.00, GETDATE(), 'WO-002', 'WORK_ORDER', 'WAREHOUSE', 'PRODUCTION', 'RAM consumption for production', 'John Doe', 1, GETDATE(), 1, 0),
(6, 'CONSUMPTION', -5.00, 150.00, -750.00, GETDATE(), 'WO-002', 'WORK_ORDER', 'WAREHOUSE', 'PRODUCTION', 'Motherboard consumption for production', 'John Doe', 1, GETDATE(), 1, 0),
(2, 'PRODUCTION', 5.00, 500.00, 2500.00, GETDATE(), 'WO-002', 'WORK_ORDER', 'PRODUCTION', 'WAREHOUSE', 'Desktop production completed', 'Jane Smith', 1, GETDATE(), 1, 0);

-- 10. Create Warehouses (at least 10 test data rows)
INSERT INTO Warehouses (Code, Name, Description, IsActive, CreatedDate, CreatedBy, IsDeleted) VALUES
('WH-001', 'Ana Depo', 'Ana merkez depo', 1, GETDATE(), 'admin', 0),
('WH-002', 'Yedek Parça Deposu', 'Yedek parça ve ekipmanlar', 1, GETDATE(), 'admin', 0),
('WH-003', 'Hammadde Deposu', 'Hammadde stokları', 1, GETDATE(), 'admin', 0),
('WH-004', 'Bitmiş Ürün Deposu', 'Bitmiş ürünlerin saklandığı depo', 1, GETDATE(), 'admin', 0),
('WH-005', 'Sevkiyat Deposu', 'Sevkiyat için ayrılmış depo', 1, GETDATE(), 'admin', 0),
('WH-006', 'Karantina Deposu', 'Karantinaya alınan ürünler', 1, GETDATE(), 'admin', 0),
('WH-007', 'Fire Deposu', 'Fire ve hurda ürünler', 1, GETDATE(), 'admin', 0),
('WH-008', 'Yedek Ekipman Deposu', 'Yedek ekipmanlar için depo', 1, GETDATE(), 'admin', 0),
('WH-009', 'Numune Deposu', 'Numune ürünlerin saklandığı depo', 1, GETDATE(), 'admin', 0),
('WH-010', 'Dış Depo', 'Dış tedarikçi deposu', 1, GETDATE(), 'admin', 0);

-- 11. Create SupplierTypes
INSERT INTO SupplierTypes (Name, Description, IsActive, CreatedDate, CreatedBy, IsDeleted) VALUES
('Hammadde', 'Hammadde tedarikçileri', 1, GETDATE(), 'admin', 0),
('Yedek Parça', 'Yedek parça tedarikçileri', 1, GETDATE(), 'admin', 0);

-- 12. Create Suppliers (at least 10)
INSERT INTO Suppliers (Name, Address, Phone, Email, TaxNumber, ContactPerson, SupplierTypeId, IsActive, CreatedDate, CreatedBy, IsDeleted) VALUES
('ABC Kimya', 'İstanbul', '0212 123 4567', 'info@abckimya.com', '1234567890', 'Ahmet Yılmaz', 1, 1, GETDATE(), 'admin', 0),
('XYZ Yedek', 'Bursa', '0224 987 6543', 'info@xyzyedek.com', '9876543210', 'Mehmet Demir', 2, 1, GETDATE(), 'admin', 0),
('Delta Metal', 'Kocaeli', '0262 111 2233', 'info@deltametal.com', '1122334455', 'Ayşe Kaya', 1, 1, GETDATE(), 'admin', 0),
('Beta Plastik', 'Ankara', '0312 555 6677', 'info@betaplastik.com', '5566778899', 'Fatih Çelik', 2, 1, GETDATE(), 'admin', 0),
('Gamma Elektronik', 'İzmir', '0232 333 4455', 'info@gammael.com', '3344556677', 'Elif Yıldız', 1, 1, GETDATE(), 'admin', 0),
('Sigma Endüstri', 'Adana', '0322 888 9999', 'info@sigmaend.com', '9988776655', 'Burak Aslan', 2, 1, GETDATE(), 'admin', 0),
('Omega Tedarik', 'Antalya', '0242 222 3344', 'info@omegatedarik.com', '2233445566', 'Cemre Güneş', 1, 1, GETDATE(), 'admin', 0),
('Epsilon Makina', 'Samsun', '0362 444 5566', 'info@epsilonmakina.com', '4455667788', 'Deniz Aksoy', 2, 1, GETDATE(), 'admin', 0),
('Zeta Kimya', 'Eskişehir', '0222 666 7788', 'info@zetakimya.com', '6677889900', 'Gökhan Polat', 1, 1, GETDATE(), 'admin', 0),
('Eta Parça', 'Gaziantep', '0342 777 8899', 'info@etaparca.com', '7788990011', 'Hale Demir', 2, 1, GETDATE(), 'admin', 0);

-- 13. Create PurchaseOrders (at least 10)
INSERT INTO PurchaseOrders (OrderNumber, SupplierId, OrderDate, TotalAmount, Status, IsActive, CreatedDate, CreatedBy, IsDeleted) VALUES
('PO-2024001', 1, '2024-07-01', 15000, 1, 1, GETDATE(), 'admin', 0),
('PO-2024002', 2, '2024-07-02', 8000, 1, 1, GETDATE(), 'admin', 0),
('PO-2024003', 3, '2024-07-03', 12000, 1, 1, GETDATE(), 'admin', 0),
('PO-2024004', 4, '2024-07-04', 9500, 1, 1, GETDATE(), 'admin', 0),
('PO-2024005', 5, '2024-07-05', 11000, 1, 1, GETDATE(), 'admin', 0),
('PO-2024006', 6, '2024-07-06', 10500, 1, 1, GETDATE(), 'admin', 0),
('PO-2024007', 7, '2024-07-07', 9800, 1, 1, GETDATE(), 'admin', 0),
('PO-2024008', 8, '2024-07-08', 10200, 1, 1, GETDATE(), 'admin', 0),
('PO-2024009', 9, '2024-07-09', 11500, 1, 1, GETDATE(), 'admin', 0),
('PO-2024010', 10, '2024-07-10', 9900, 1, 1, GETDATE(), 'admin', 0);

-- 14. Create Customers (at least 10)
INSERT INTO Customers (Name, Address, Phone, Email, TaxNumber, ContactPerson, IsActive, CreatedDate, CreatedBy, IsDeleted) VALUES
('Teklas Otomotiv', 'Kocaeli', '0262 111 2233', 'info@teklas.com', '1122334455', 'Ayşe Kaya', 1, GETDATE(), 'admin', 0),
('Beta Makina', 'Ankara', '0312 555 6677', 'info@betamakina.com', '5566778899', 'Fatih Çelik', 1, GETDATE(), 'admin', 0),
('Gamma Elektrik', 'İstanbul', '0212 123 4567', 'info@gammael.com', '3344556677', 'Elif Yıldız', 1, GETDATE(), 'admin', 0),
('Delta Endüstri', 'Bursa', '0224 987 6543', 'info@deltaend.com', '9988776655', 'Burak Aslan', 1, GETDATE(), 'admin', 0),
('Omega Sanayi', 'İzmir', '0232 333 4455', 'info@omegasanayi.com', '2233445566', 'Cemre Güneş', 1, GETDATE(), 'admin', 0),
('Sigma Parça', 'Adana', '0322 888 9999', 'info@sigmaparca.com', '4455667788', 'Deniz Aksoy', 1, GETDATE(), 'admin', 0),
('Epsilon Kimya', 'Antalya', '0242 222 3344', 'info@epsilonkimya.com', '6677889900', 'Gökhan Polat', 1, GETDATE(), 'admin', 0),
('Zeta Plastik', 'Samsun', '0362 444 5566', 'info@zetaplastik.com', '7788990011', 'Hale Demir', 1, GETDATE(), 'admin', 0),
('Eta Metal', 'Eskişehir', '0222 666 7788', 'info@etapmetal.com', '8899001122', 'Mert Yılmaz', 1, GETDATE(), 'admin', 0),
('Theta Elektronik', 'Gaziantep', '0342 777 8899', 'info@thetael.com', '9900112233', 'Selin Acar', 1, GETDATE(), 'admin', 0);

-- 15. Create CustomerOrders (at least 10)
INSERT INTO CustomerOrders (OrderNumber, CustomerId, OrderDate, TotalAmount, Status, IsActive, CreatedDate, CreatedBy, IsDeleted) VALUES
('SO-2024001', 1, '2024-07-03', 20000, 1, 1, GETDATE(), 'admin', 0),
('SO-2024002', 2, '2024-07-04', 12000, 1, 1, GETDATE(), 'admin', 0),
('SO-2024003', 3, '2024-07-05', 18000, 1, 1, GETDATE(), 'admin', 0),
('SO-2024004', 4, '2024-07-06', 9500, 1, 1, GETDATE(), 'admin', 0),
('SO-2024005', 5, '2024-07-07', 11000, 1, 1, GETDATE(), 'admin', 0),
('SO-2024006', 6, '2024-07-08', 10500, 1, 1, GETDATE(), 'admin', 0),
('SO-2024007', 7, '2024-07-09', 9800, 1, 1, GETDATE(), 'admin', 0),
('SO-2024008', 8, '2024-07-10', 10200, 1, 1, GETDATE(), 'admin', 0),
('SO-2024009', 9, '2024-07-11', 11500, 1, 1, GETDATE(), 'admin', 0),
('SO-2024010', 10, '2024-07-12', 9900, 1, 1, GETDATE(), 'admin', 0);

-- Check what we created
SELECT 'Material Categories: ' + CAST(COUNT(*) AS VARCHAR) FROM MaterialCategories;
SELECT 'Material Cards: ' + CAST(COUNT(*) AS VARCHAR) FROM MaterialCards;
SELECT 'BOM Headers: ' + CAST(COUNT(*) AS VARCHAR) FROM BOMHeaders;
SELECT 'BOM Items: ' + CAST(COUNT(*) AS VARCHAR) FROM BOMItems;
SELECT 'Work Orders: ' + CAST(COUNT(*) AS VARCHAR) FROM WorkOrders;
SELECT 'Work Order Operations: ' + CAST(COUNT(*) AS VARCHAR) FROM WorkOrderOperations;
SELECT 'Production Confirmations: ' + CAST(COUNT(*) AS VARCHAR) FROM ProductionConfirmations;
SELECT 'Material Consumptions: ' + CAST(COUNT(*) AS VARCHAR) FROM MaterialConsumptions;
SELECT 'Material Movements: ' + CAST(COUNT(*) AS VARCHAR) FROM MaterialMovements; 