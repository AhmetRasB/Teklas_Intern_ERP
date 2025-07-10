-- ===================================================================
-- CORRECTED TEST DATA FOR PRODUCTION MANAGEMENT
-- ===================================================================

-- First, create necessary Material Cards for testing
INSERT INTO MaterialCards (
    CardCode, CardName, CardType, Unit, 
    MaterialCategoryId, PurchasePrice, SalesPrice, CurrentStock,
    Description, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
-- Finished Products
('LAPTOP-001', 'Gaming Laptop Pro', 'FINISHED_PRODUCT', 'EACH', 1, 800.00, 1200.00, 0, 'High-performance gaming laptop', GETDATE(), 1, 0, 1),
('PHONE-001', 'Smartphone Elite', 'FINISHED_PRODUCT', 'EACH', 1, 300.00, 600.00, 0, 'Flagship smartphone model', GETDATE(), 1, 0, 1),
('TABLET-001', 'Professional Tablet', 'FINISHED_PRODUCT', 'EACH', 1, 200.00, 400.00, 0, 'Business tablet device', GETDATE(), 1, 0, 1),

-- Components
('CPU-001', 'Intel i7 Processor', 'COMPONENT', 'EACH', 1, 200.00, 250.00, 100, 'High-performance processor', GETDATE(), 1, 0, 1),
('GPU-001', 'NVIDIA RTX Graphics', 'COMPONENT', 'EACH', 1, 300.00, 350.00, 50, 'Gaming graphics card', GETDATE(), 1, 0, 1),
('RAM-001', 'DDR4 RAM Module', 'COMPONENT', 'GB', 1, 5.00, 8.00, 500, 'High-speed memory module', GETDATE(), 1, 0, 1),
('SSD-001', '1TB NVMe SSD', 'COMPONENT', 'EACH', 1, 80.00, 120.00, 200, 'Fast storage device', GETDATE(), 1, 0, 1),
('BAT-001', 'Lithium Battery', 'COMPONENT', 'EACH', 1, 20.00, 35.00, 300, 'Long-lasting battery', GETDATE(), 1, 0, 1),
('DISP-001', 'OLED Display', 'COMPONENT', 'EACH', 1, 50.00, 80.00, 150, 'High-quality display', GETDATE(), 1, 0, 1),
('CAM-001', 'Camera Module', 'COMPONENT', 'EACH', 1, 25.00, 40.00, 200, 'Advanced camera system', GETDATE(), 1, 0, 1);

-- Create a simple BOM for Laptop
INSERT INTO BillOfMaterials (
    BOMCode, BOMName, Version, ProductMaterialCardId, BaseQuantity, Unit,
    BOMType, Description, ApprovalStatus,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('BOM-LAPTOP-001', 'Gaming Laptop Assembly', 'v1.0', 1, 1.00, 'EACH',
 'ASSEMBLY', 'Gaming laptop production', 'APPROVED',
 GETDATE(), 1, 0, 1);

-- Add BOM Items
INSERT INTO BillOfMaterialItems (
    BillOfMaterialId, MaterialCardId, Quantity, Unit, ScrapFactor, 
    ComponentType, IssueMethod, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(1, 4, 1.00, 'EACH', 0.02, 'COMPONENT', 'PICK_LIST', 'Processor', GETDATE(), 1, 0, 1),
(1, 5, 1.00, 'EACH', 0.01, 'COMPONENT', 'PICK_LIST', 'Graphics Card', GETDATE(), 1, 0, 1),
(1, 6, 16.00, 'GB', 0.01, 'COMPONENT', 'PICK_LIST', 'Memory', GETDATE(), 1, 0, 1),
(1, 7, 1.00, 'EACH', 0.01, 'COMPONENT', 'PICK_LIST', 'Storage', GETDATE(), 1, 0, 1),
(1, 8, 1.00, 'EACH', 0.05, 'COMPONENT', 'PICK_LIST', 'Battery', GETDATE(), 1, 0, 1);

-- Create Work Order
INSERT INTO WorkOrders (
    WorkOrderNumber, BillOfMaterialId, ProductMaterialCardId, PlannedQuantity, 
    CompletedQuantity, ScrapQuantity, Unit, Status, Priority, 
    PlannedStartDate, PlannedEndDate, Description, WorkCenter,
    WorkOrderType, CompletionPercentage, RequiresQualityCheck, QualityStatus,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('WO-TEST-001', 1, 1, 10.00, 5.00, 1.00, 'EACH', 'IN_PROGRESS', 1,
 DATEADD(day, -1, GETDATE()), DATEADD(day, 2, GETDATE()), 
 'Test laptop production order', 'ASSEMBLY_LINE_1',
 'PRODUCTION', 60.00, 1, 'PENDING',
 GETDATE(), 1, 0);

-- Create Production Confirmation
INSERT INTO ProductionConfirmations (
    ConfirmationNumber, WorkOrderId, ConfirmedQuantity, ScrapQuantity, ReworkQuantity,
    Unit, ConfirmationDate, WorkCenter, Status, ConfirmationType,
    OperatorUserId, Notes, QualityStatus, ActivityType,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('PC-TEST-001', 1, 5.00, 1.00, 0.00, 'EACH', GETDATE(), 'ASSEMBLY_LINE_1', 
 'CONFIRMED', 'PARTIAL', 1, 'Test production batch completed', 'PASSED', 'ASSEMBLY',
 GETDATE(), 1, 0); 