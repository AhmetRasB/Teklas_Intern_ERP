-- ===================================================================
-- FINAL TEST DATA - ALL REQUIRED FIELDS
-- ===================================================================

-- 1. Create Material Categories first (with all required fields)
INSERT INTO MaterialCategories (
    CategoryCode, CategoryName, Description, ParentCategoryId, Level, DisplayOrder,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('CAT-001', 'Electronics', 'Electronic products and components', NULL, 1, 1, GETDATE(), 1, 0, 1),
('CAT-002', 'Components', 'Raw components for assembly', NULL, 1, 2, GETDATE(), 1, 0, 1);

-- 2. Create Material Cards
INSERT INTO MaterialCards (
    CardCode, CardName, CardType, Unit, MaterialCategoryId, 
    PurchasePrice, SalesPrice, CurrentStock, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
-- Finished Products
('LAPTOP-001', 'Gaming Laptop', 'FINISHED_PRODUCT', 'EACH', 1, 800.00, 1200.00, 0, 'Gaming laptop', GETDATE(), 1, 0, 1),
-- Components  
('CPU-001', 'Processor', 'COMPONENT', 'EACH', 2, 200.00, 250.00, 100, 'CPU', GETDATE(), 1, 0, 1),
('GPU-001', 'Graphics Card', 'COMPONENT', 'EACH', 2, 300.00, 350.00, 50, 'GPU', GETDATE(), 1, 0, 1);

-- 3. Create BOM
INSERT INTO BillOfMaterials (
    BOMCode, BOMName, Version, ProductMaterialCardId, BaseQuantity, Unit,
    BOMType, Description, ApprovalStatus, IsActive,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('BOM-001', 'Laptop Assembly', 'v1.0', 1, 1.00, 'EACH',
 'ASSEMBLY', 'Basic laptop assembly', 'APPROVED', 1,
 GETDATE(), 1, 0, 1);

-- 4. Add BOM Items (need to check required fields)
INSERT INTO BillOfMaterialItems (
    BillOfMaterialId, MaterialCardId, LineNumber, Quantity, Unit, ScrapFactor, 
    ComponentType, IssueMethod, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(1, 2, 1, 1.00, 'EACH', 0.02, 'COMPONENT', 'PICK_LIST', 'Processor', GETDATE(), 1, 0, 1),
(1, 3, 2, 1.00, 'EACH', 0.01, 'COMPONENT', 'PICK_LIST', 'Graphics Card', GETDATE(), 1, 0, 1);

-- 5. Create Work Order
INSERT INTO WorkOrders (
    WorkOrderNumber, BillOfMaterialId, ProductMaterialCardId, PlannedQuantity, 
    CompletedQuantity, ScrapQuantity, Unit, Status, Priority, 
    PlannedStartDate, PlannedEndDate, Description, WorkCenter,
    WorkOrderType, CompletionPercentage, RequiresQualityCheck, QualityStatus,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('WO-TEST-001', 1, 1, 10.00, 3.00, 0.00, 'EACH', 'IN_PROGRESS', 1,
 DATEADD(day, -1, GETDATE()), DATEADD(day, 2, GETDATE()), 
 'Test laptop production', 'ASSEMBLY_LINE_1',
 'PRODUCTION', 30.00, 1, 'PENDING',
 GETDATE(), 1, 0);

-- 6. Create Production Confirmation
INSERT INTO ProductionConfirmations (
    ConfirmationNumber, WorkOrderId, ConfirmedQuantity, ScrapQuantity, ReworkQuantity,
    Unit, ConfirmationDate, WorkCenter, Status, ConfirmationType,
    OperatorUserId, Notes, QualityStatus, ActivityType, RequiresQualityCheck,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('PC-TEST-001', 1, 3.00, 0.00, 0.00, 'EACH', GETDATE(), 'ASSEMBLY_LINE_1', 
 'CONFIRMED', 'PARTIAL', 1, 'Test production completed', 'PASSED', 'ASSEMBLY', 0,
 GETDATE(), 1, 0);

-- Verification
PRINT 'Test data creation completed!';
SELECT COUNT(*) as 'Material Categories' FROM MaterialCategories;
SELECT COUNT(*) as 'Material Cards' FROM MaterialCards;
SELECT COUNT(*) as 'BOMs' FROM BillOfMaterials;
SELECT COUNT(*) as 'BOM Items' FROM BillOfMaterialItems;
SELECT COUNT(*) as 'Work Orders' FROM WorkOrders;
SELECT COUNT(*) as 'Production Confirmations' FROM ProductionConfirmations; 