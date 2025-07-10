-- ===================================================================
-- COMPLETE TEST DATA FOR ALL TABLES - 5 RECORDS EACH
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
    CategoryCode, CategoryName, Description, ParentCategoryId, Level, DisplayOrder,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('CAT-001', 'Electronics', 'Electronic products and components', NULL, 1, 1, GETDATE(), 1, 0, 1),
('CAT-002', 'Components', 'Raw components for assembly', NULL, 1, 2, GETDATE(), 1, 0, 1),
('CAT-003', 'Hardware', 'Computer hardware components', NULL, 1, 3, GETDATE(), 1, 0, 1),
('CAT-004', 'Software', 'Software products', NULL, 1, 4, GETDATE(), 1, 0, 1),
('CAT-005', 'Accessories', 'Various accessories', NULL, 1, 5, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 2. USERS (5 records)
-- ===================================================================
INSERT INTO Users (
    Username, Email, PasswordHash, FirstName, LastName, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('admin', 'admin@teklas.com', 'hashedpassword1', 'Admin', 'User', GETDATE(), 1, 0, 1),
('operator1', 'operator1@teklas.com', 'hashedpassword2', 'John', 'Doe', GETDATE(), 1, 0, 1),
('operator2', 'operator2@teklas.com', 'hashedpassword3', 'Jane', 'Smith', GETDATE(), 1, 0, 1),
('supervisor', 'supervisor@teklas.com', 'hashedpassword4', 'Bob', 'Wilson', GETDATE(), 1, 0, 1),
('quality', 'quality@teklas.com', 'hashedpassword5', 'Alice', 'Johnson', GETDATE(), 1, 0, 1);

-- ===================================================================
-- 3. ROLES (5 records)
-- ===================================================================
INSERT INTO Roles (
    RoleName, Description, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('Admin', 'System Administrator', GETDATE(), 1, 0, 1),
('ProductionOperator', 'Production Line Operator', GETDATE(), 1, 0, 1),
('Supervisor', 'Production Supervisor', GETDATE(), 1, 0, 1),
('QualityControl', 'Quality Control Inspector', GETDATE(), 1, 0, 1),
('Manager', 'Production Manager', GETDATE(), 1, 0, 1);

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
('RAM-001', 'DDR4 RAM Module', 'COMPONENT', 'GB', 3, 5.00, 8.00, 200, 'High-speed memory module', GETDATE(), 1, 0, 1),
('SSD-001', '1TB NVMe SSD', 'COMPONENT', 'EACH', 3, 80.00, 120.00, 100, 'Fast storage device', GETDATE(), 1, 0, 1);

-- ===================================================================
-- 5. USER ROLES (5 records)
-- ===================================================================
INSERT INTO UserRoles (
    UserId, RoleId, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(1, 1, GETDATE(), 1, 0, 1), -- admin -> Admin
(2, 2, GETDATE(), 1, 0, 1), -- operator1 -> ProductionOperator
(3, 2, GETDATE(), 1, 0, 1), -- operator2 -> ProductionOperator
(4, 3, GETDATE(), 1, 0, 1), -- supervisor -> Supervisor
(5, 4, GETDATE(), 1, 0, 1); -- quality -> QualityControl

-- ===================================================================
-- 6. BILL OF MATERIALS (5 records)
-- ===================================================================
INSERT INTO BillOfMaterials (
    BOMCode, BOMName, Version, ProductMaterialCardId, BaseQuantity, Unit,
    BOMType, Description, ApprovalStatus, IsActive,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('BOM-LAPTOP-001', 'Gaming Laptop Assembly', 'v1.0', 1, 1.00, 'EACH', 'ASSEMBLY', 'Gaming laptop production', 'APPROVED', 1, GETDATE(), 1, 0, 1),
('BOM-LAPTOP-002', 'Business Laptop Assembly', 'v1.0', 1, 1.00, 'EACH', 'ASSEMBLY', 'Business laptop production', 'APPROVED', 1, GETDATE(), 1, 0, 1),
('BOM-LAPTOP-003', 'Student Laptop Assembly', 'v1.0', 1, 1.00, 'EACH', 'ASSEMBLY', 'Student laptop production', 'PENDING', 1, GETDATE(), 1, 0, 1),
('BOM-LAPTOP-004', 'Gaming Pro Max Assembly', 'v2.0', 1, 1.00, 'EACH', 'ASSEMBLY', 'High-end gaming laptop', 'APPROVED', 1, GETDATE(), 1, 0, 1),
('BOM-LAPTOP-005', 'Ultra Thin Assembly', 'v1.0', 1, 1.00, 'EACH', 'ASSEMBLY', 'Ultra thin laptop production', 'DRAFT', 0, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 7. BILL OF MATERIAL ITEMS (5 records per BOM = 25 total)
-- ===================================================================
INSERT INTO BillOfMaterialItems (
    BillOfMaterialId, MaterialCardId, LineNumber, Quantity, Unit, ScrapFactor, 
    ComponentType, IssueMethod, Description, IsOptional,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
-- BOM 1 Items
(1, 2, 1, 1.00, 'EACH', 2.0, 'COMPONENT', 'PICK_LIST', 'Processor', 0, GETDATE(), 1, 0, 1),
(1, 3, 2, 1.00, 'EACH', 1.0, 'COMPONENT', 'PICK_LIST', 'Graphics Card', 0, GETDATE(), 1, 0, 1),
(1, 4, 3, 16.00, 'GB', 1.0, 'COMPONENT', 'PICK_LIST', 'Memory', 0, GETDATE(), 1, 0, 1),
(1, 5, 4, 1.00, 'EACH', 1.0, 'COMPONENT', 'PICK_LIST', 'Storage', 0, GETDATE(), 1, 0, 1),
(1, 2, 5, 0.50, 'EACH', 5.0, 'COMPONENT', 'PICK_LIST', 'Backup CPU', 1, GETDATE(), 1, 0, 1),

-- BOM 2 Items
(2, 2, 1, 1.00, 'EACH', 2.0, 'COMPONENT', 'PICK_LIST', 'Business Processor', 0, GETDATE(), 1, 0, 1),
(2, 4, 2, 8.00, 'GB', 1.0, 'COMPONENT', 'PICK_LIST', 'Business Memory', 0, GETDATE(), 1, 0, 1),
(2, 5, 3, 1.00, 'EACH', 1.0, 'COMPONENT', 'PICK_LIST', 'Business Storage', 0, GETDATE(), 1, 0, 1),
(2, 3, 4, 0.50, 'EACH', 2.0, 'COMPONENT', 'PICK_LIST', 'Basic Graphics', 1, GETDATE(), 1, 0, 1),
(2, 2, 5, 0.25, 'EACH', 5.0, 'COMPONENT', 'PICK_LIST', 'Spare Processor', 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 8. WORK ORDERS (5 records)
-- ===================================================================
INSERT INTO WorkOrders (
    WorkOrderNumber, BillOfMaterialId, ProductMaterialCardId, PlannedQuantity, 
    CompletedQuantity, ScrapQuantity, Unit, Status, Priority, 
    PlannedStartDate, PlannedEndDate, Description, WorkCenter,
    WorkOrderType, CompletionPercentage, RequiresQualityCheck, QualityStatus,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('WO-2024-001', 1, 1, 10.00, 7.00, 1.00, 'EACH', 'IN_PROGRESS', 1, DATEADD(day, -2, GETDATE()), DATEADD(day, 3, GETDATE()), 'Gaming laptop production batch 1', 'ASSEMBLY_LINE_1', 'PRODUCTION', 80.00, 1, 'PENDING', GETDATE(), 1, 0),
('WO-2024-002', 2, 1, 5.00, 0.00, 0.00, 'EACH', 'CREATED', 2, GETDATE(), DATEADD(day, 2, GETDATE()), 'Business laptop production', 'ASSEMBLY_LINE_2', 'PRODUCTION', 0.00, 1, 'NOT_REQUIRED', GETDATE(), 1, 0),
('WO-2024-003', 1, 1, 15.00, 15.00, 2.00, 'EACH', 'COMPLETED', 3, DATEADD(day, -5, GETDATE()), DATEADD(day, -1, GETDATE()), 'Gaming laptop production batch 2', 'ASSEMBLY_LINE_1', 'PRODUCTION', 100.00, 1, 'PASSED', GETDATE(), 1, 0),
('WO-2024-004', 4, 1, 3.00, 1.00, 0.00, 'EACH', 'IN_PROGRESS', 1, DATEADD(day, -1, GETDATE()), DATEADD(day, 4, GETDATE()), 'Gaming Pro Max production', 'ASSEMBLY_LINE_3', 'PRODUCTION', 33.33, 1, 'PENDING', GETDATE(), 1, 0),
('WO-2024-005', 2, 1, 20.00, 0.00, 0.00, 'EACH', 'RELEASED', 2, DATEADD(day, 1, GETDATE()), DATEADD(day, 7, GETDATE()), 'Large business laptop order', 'ASSEMBLY_LINE_2', 'PRODUCTION', 0.00, 1, 'NOT_REQUIRED', GETDATE(), 1, 0);

-- ===================================================================
-- 9. PRODUCTION CONFIRMATIONS (5 records)
-- ===================================================================
INSERT INTO ProductionConfirmations (
    ConfirmationNumber, WorkOrderId, ConfirmedQuantity, ScrapQuantity, ReworkQuantity,
    Unit, ConfirmationDate, WorkCenter, Status, ConfirmationType,
    OperatorUserId, Notes, QualityStatus, ActivityType, RequiresQualityCheck, MaterialConsumed,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('PC-2024-001', 1, 3.00, 0.00, 0.00, 'EACH', DATEADD(day, -1, GETDATE()), 'ASSEMBLY_LINE_1', 'CONFIRMED', 'PARTIAL', 2, 'First batch completed successfully', 'PASSED', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0),
('PC-2024-002', 1, 4.00, 1.00, 0.00, 'EACH', GETDATE(), 'ASSEMBLY_LINE_1', 'CONFIRMED', 'PARTIAL', 3, 'Second batch with minor issues', 'PASSED', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0),
('PC-2024-003', 3, 15.00, 2.00, 0.00, 'EACH', DATEADD(day, -1, GETDATE()), 'ASSEMBLY_LINE_1', 'POSTED', 'FINAL', 2, 'Batch completed successfully', 'PASSED', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0),
('PC-2024-004', 4, 1.00, 0.00, 0.00, 'EACH', GETDATE(), 'ASSEMBLY_LINE_3', 'CONFIRMED', 'PARTIAL', 4, 'Gaming Pro Max first unit', 'PASSED', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0),
('PC-2024-005', 1, 0.00, 0.00, 1.00, 'EACH', GETDATE(), 'ASSEMBLY_LINE_1', 'DRAFT', 'REWORK', 2, 'Unit requires rework', 'FAILED', 'REWORK', 1, 0, GETDATE(), 1, 0);

-- ===================================================================
-- 10. MATERIAL MOVEMENTS (5 records)
-- ===================================================================
INSERT INTO MaterialMovements (
    MovementCode, MaterialCardId, MovementType, Quantity, Unit, 
    ReferenceType, ReferenceId, SourceLocation, TargetLocation, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('MOV-001', 2, 'IN', 50.00, 'EACH', 'PURCHASE_ORDER', 1001, 'WAREHOUSE', 'PRODUCTION_LINE_1', 'CPU delivery for production', GETDATE(), 1, 0, 1),
('MOV-002', 3, 'IN', 25.00, 'EACH', 'PURCHASE_ORDER', 1002, 'WAREHOUSE', 'PRODUCTION_LINE_1', 'GPU delivery for production', GETDATE(), 1, 0, 1),
('MOV-003', 4, 'OUT', 80.00, 'GB', 'WORK_ORDER', 1, 'WAREHOUSE', 'ASSEMBLY_LINE_1', 'RAM consumption for WO-2024-001', GETDATE(), 1, 0, 1),
('MOV-004', 5, 'OUT', 10.00, 'EACH', 'WORK_ORDER', 1, 'WAREHOUSE', 'ASSEMBLY_LINE_1', 'SSD consumption for WO-2024-001', GETDATE(), 1, 0, 1),
('MOV-005', 1, 'OUT', 7.00, 'EACH', 'SALES_ORDER', 2001, 'FINISHED_GOODS', 'SHIPPING', 'Laptop shipment to customer', GETDATE(), 1, 0, 1);

-- ===================================================================
-- VERIFICATION QUERIES
-- ===================================================================
PRINT '=== TEST DATA CREATION COMPLETED ===';
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

PRINT '=== SAMPLE DATA OVERVIEW ===';
PRINT 'Work Orders Status Distribution:';
SELECT Status, COUNT(*) as Count FROM WorkOrders GROUP BY Status;

PRINT 'Production Summary:';
SELECT 
    wo.WorkOrderNumber,
    wo.PlannedQuantity,
    wo.CompletedQuantity,
    wo.Status,
    bom.BOMName
FROM WorkOrders wo
JOIN BillOfMaterials bom ON wo.BillOfMaterialId = bom.Id
ORDER BY wo.WorkOrderNumber; 