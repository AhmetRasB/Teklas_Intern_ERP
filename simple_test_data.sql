-- ===================================================================
-- SIMPLE TEST DATA - ESSENTIAL COLUMNS ONLY 
-- ===================================================================

-- Clear existing data in dependency order
DELETE FROM ProductionConfirmations;
DELETE FROM MaterialMovements;
DELETE FROM WorkOrders;
DELETE FROM BillOfMaterialItems;
DELETE FROM BillOfMaterials;
DELETE FROM UserRoles;
DELETE FROM MaterialCards;
DELETE FROM MaterialCategories;
DELETE FROM Roles;
DELETE FROM Users;

-- ===================================================================
-- 1. MATERIAL CATEGORIES (5 records)
-- ===================================================================
INSERT INTO MaterialCategories (
    CategoryCode, CategoryName, Description, Level, DisplayOrder,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('CAT-001', 'Electronics', 'Electronic products', 1, 1, GETDATE(), 1, 0, 1),
('CAT-002', 'Components', 'Raw components', 1, 2, GETDATE(), 1, 0, 1),
('CAT-003', 'Hardware', 'Computer hardware', 1, 3, GETDATE(), 1, 0, 1),
('CAT-004', 'Software', 'Software products', 1, 4, GETDATE(), 1, 0, 1),
('CAT-005', 'Accessories', 'Various accessories', 1, 5, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 2. USERS (5 records)
-- ===================================================================
INSERT INTO Users (
    Username, Email, PasswordHash, FirstName, LastName, 
    IsActive, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('admin', 'admin@teklas.com', 'hashedpassword1', 'Admin', 'User', 1, GETDATE(), 1, 0, 1),
('operator1', 'operator1@teklas.com', 'hashedpassword2', 'John', 'Doe', 1, GETDATE(), 1, 0, 1),
('operator2', 'operator2@teklas.com', 'hashedpassword3', 'Jane', 'Smith', 1, GETDATE(), 1, 0, 1),
('supervisor', 'supervisor@teklas.com', 'hashedpassword4', 'Bob', 'Wilson', 1, GETDATE(), 1, 0, 1),
('quality', 'quality@teklas.com', 'hashedpassword5', 'Alice', 'Johnson', 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 3. ROLES (5 records)
-- ===================================================================
INSERT INTO Roles (
    Name, DisplayName, Description, IsActive,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('Admin', 'System Administrator', 'System Administrator', 1, GETDATE(), 1, 0, 1),
('ProductionOperator', 'Production Operator', 'Production Line Operator', 1, GETDATE(), 1, 0, 1),
('Supervisor', 'Production Supervisor', 'Production Supervisor', 1, GETDATE(), 1, 0, 1),
('QualityControl', 'Quality Control', 'Quality Control Inspector', 1, GETDATE(), 1, 0, 1),
('Manager', 'Production Manager', 'Production Manager', 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 4. MATERIAL CARDS (5 records)
-- ===================================================================
INSERT INTO MaterialCards (
    CardCode, CardName, CardType, Unit, MaterialCategoryId, 
    PurchasePrice, SalesPrice, CurrentStock, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('LAPTOP-001', 'Gaming Laptop Pro', 'FINISHED_PRODUCT', 'EACH', 1, 800.00, 1200.00, 10, 'High-performance gaming laptop', GETDATE(), 1, 0, 1),
('CPU-001', 'Intel i7 Processor', 'COMPONENT', 'EACH', 2, 200.00, 250.00, 50, 'High-performance processor', GETDATE(), 1, 0, 1),
('GPU-001', 'NVIDIA RTX Graphics', 'COMPONENT', 'EACH', 3, 300.00, 350.00, 25, 'Gaming graphics card', GETDATE(), 1, 0, 1),
('RAM-001', 'DDR4 RAM Module', 'COMPONENT', 'EACH', 3, 5.00, 8.00, 200, 'High-speed memory module', GETDATE(), 1, 0, 1),
('SSD-001', '1TB NVMe SSD', 'COMPONENT', 'EACH', 3, 80.00, 120.00, 100, 'Fast storage device', GETDATE(), 1, 0, 1);

-- ===================================================================
-- 5. USER ROLES (5 records)
-- ===================================================================
INSERT INTO UserRoles (
    UserId, RoleId, IsActive, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(1, 1, 1, GETDATE(), 1, 0, 1), -- admin -> Admin
(2, 2, 1, GETDATE(), 1, 0, 1), -- operator1 -> ProductionOperator
(3, 2, 1, GETDATE(), 1, 0, 1), -- operator2 -> ProductionOperator
(4, 3, 1, GETDATE(), 1, 0, 1), -- supervisor -> Supervisor
(5, 4, 1, GETDATE(), 1, 0, 1); -- quality -> QualityControl

-- ===================================================================
-- 6. BILL OF MATERIALS (3 records - essential columns only)
-- ===================================================================
INSERT INTO BillOfMaterials (
    BOMCode, BOMName, Version, ProductMaterialCardId, BaseQuantity, Unit,
    BOMType, Description, ApprovalStatus, IsActive,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('BOM-LAPTOP-001', 'Gaming Laptop Assembly', 'v1.0', 1, 1.00, 'EACH', 'ASSEMBLY', 'Gaming laptop production', 'APPROVED', 1, GETDATE(), 1, 0, 1),
('BOM-LAPTOP-002', 'Business Laptop Assembly', 'v1.0', 1, 1.00, 'EACH', 'ASSEMBLY', 'Business laptop production', 'APPROVED', 1, GETDATE(), 1, 0, 1),
('BOM-LAPTOP-003', 'Student Laptop Assembly', 'v1.0', 1, 1.00, 'EACH', 'ASSEMBLY', 'Student laptop production', 'PENDING', 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 7. BILL OF MATERIAL ITEMS (6 records - essential columns only)
-- ===================================================================
INSERT INTO BillOfMaterialItems (
    BillOfMaterialId, MaterialCardId, LineNumber, Quantity, Unit, 
    ComponentType, IssueMethod, Description, IsOptional,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
-- BOM 1 Items
(1, 2, 1, 1.00, 'EACH', 'COMPONENT', 'PICK_LIST', 'Processor', 0, GETDATE(), 1, 0, 1),
(1, 3, 2, 1.00, 'EACH', 'COMPONENT', 'PICK_LIST', 'Graphics Card', 0, GETDATE(), 1, 0, 1),
(1, 4, 3, 16.00, 'EACH', 'COMPONENT', 'PICK_LIST', 'Memory', 0, GETDATE(), 1, 0, 1),
(1, 5, 4, 1.00, 'EACH', 'COMPONENT', 'PICK_LIST', 'Storage', 0, GETDATE(), 1, 0, 1),
-- BOM 2 Items
(2, 2, 1, 1.00, 'EACH', 'COMPONENT', 'PICK_LIST', 'Business Processor', 0, GETDATE(), 1, 0, 1),
(2, 4, 2, 8.00, 'EACH', 'COMPONENT', 'PICK_LIST', 'Business Memory', 0, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 8. WORK ORDERS (3 records - essential columns only)
-- ===================================================================
INSERT INTO WorkOrders (
    WorkOrderNumber, BillOfMaterialId, ProductMaterialCardId, PlannedQuantity, 
    CompletedQuantity, ScrapQuantity, Unit, Status, Priority, 
    PlannedStartDate, PlannedEndDate, Description, WorkCenter,
    WorkOrderType, CompletionPercentage, RequiresQualityCheck,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('WO-2024-001', 1, 1, 10.00, 7.00, 1.00, 'EACH', 'IN_PROGRESS', 1, DATEADD(day, -2, GETDATE()), DATEADD(day, 3, GETDATE()), 'Gaming laptop production', 'ASSEMBLY_LINE_1', 'PRODUCTION', 80.00, 1, GETDATE(), 1, 0),
('WO-2024-002', 2, 1, 5.00, 0.00, 0.00, 'EACH', 'CREATED', 2, GETDATE(), DATEADD(day, 2, GETDATE()), 'Business laptop production', 'ASSEMBLY_LINE_2', 'PRODUCTION', 0.00, 1, GETDATE(), 1, 0),
('WO-2024-003', 1, 1, 15.00, 15.00, 2.00, 'EACH', 'COMPLETED', 3, DATEADD(day, -5, GETDATE()), DATEADD(day, -1, GETDATE()), 'Gaming laptop production', 'ASSEMBLY_LINE_1', 'PRODUCTION', 100.00, 1, GETDATE(), 1, 0);

-- ===================================================================
-- 9. PRODUCTION CONFIRMATIONS (3 records - essential columns only)
-- ===================================================================
INSERT INTO ProductionConfirmations (
    ConfirmationNumber, WorkOrderId, ConfirmedQuantity, ScrapQuantity, ReworkQuantity,
    Unit, ConfirmationDate, WorkCenter, Status, ConfirmationType,
    OperatorUserId, Notes, ActivityType, RequiresQualityCheck, MaterialConsumed,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('PC-2024-001', 1, 3.00, 0.00, 0.00, 'EACH', DATEADD(day, -1, GETDATE()), 'ASSEMBLY_LINE_1', 'CONFIRMED', 'PARTIAL', 2, 'First batch completed', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0),
('PC-2024-002', 1, 4.00, 1.00, 0.00, 'EACH', GETDATE(), 'ASSEMBLY_LINE_1', 'CONFIRMED', 'PARTIAL', 3, 'Second batch', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0),
('PC-2024-003', 3, 15.00, 2.00, 0.00, 'EACH', DATEADD(day, -1, GETDATE()), 'ASSEMBLY_LINE_1', 'POSTED', 'FINAL', 2, 'Batch completed', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0);

-- ===================================================================
-- 10. MATERIAL MOVEMENTS (3 records - corrected columns)
-- ===================================================================
INSERT INTO MaterialMovements (
    MaterialCardId, MovementType, Quantity, ReferenceNumber,
    ReferenceType, LocationFrom, LocationTo, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(2, 'IN', 50.00, '1001', 'PURCHASE_ORDER', 'WAREHOUSE', 'PRODUCTION_LINE_1', 'CPU delivery', GETDATE(), 1, 0, 1),
(3, 'IN', 25.00, '1002', 'PURCHASE_ORDER', 'WAREHOUSE', 'PRODUCTION_LINE_1', 'GPU delivery', GETDATE(), 1, 0, 1),
(4, 'OUT', 80.00, '1', 'WORK_ORDER', 'WAREHOUSE', 'ASSEMBLY_LINE_1', 'RAM consumption', GETDATE(), 1, 0, 1);

-- ===================================================================
-- VERIFICATION QUERIES
-- ===================================================================
PRINT '=== SIMPLE TEST DATA CREATION COMPLETED ===';
SELECT 'MaterialCategories' as TableName, COUNT(*) as RecordCount FROM MaterialCategories
UNION ALL SELECT 'Users', COUNT(*) FROM Users
UNION ALL SELECT 'Roles', COUNT(*) FROM Roles
UNION ALL SELECT 'MaterialCards', COUNT(*) FROM MaterialCards
UNION ALL SELECT 'UserRoles', COUNT(*) FROM UserRoles
UNION ALL SELECT 'BillOfMaterials', COUNT(*) FROM BillOfMaterials
UNION ALL SELECT 'BillOfMaterialItems', COUNT(*) FROM BillOfMaterialItems
UNION ALL SELECT 'WorkOrders', COUNT(*) FROM WorkOrders
UNION ALL SELECT 'ProductionConfirmations', COUNT(*) FROM ProductionConfirmations
UNION ALL SELECT 'MaterialMovements', COUNT(*) FROM MaterialMovements
ORDER BY TableName; 