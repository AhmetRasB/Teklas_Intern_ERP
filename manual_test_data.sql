-- ===================================================================
-- MANUAL MINIMAL TEST DATA - HANDLING ALL NULL CONSTRAINTS
-- ===================================================================

-- ===================================================================
-- 1. USERS (5 records) - Add required fields
-- ===================================================================
INSERT INTO Users (
    Username, Email, PasswordHash, PasswordSalt, FirstName, LastName, 
    IsActive, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('admin', 'admin@teklas.com', 'hashedpassword1', 'salt1', 'Admin', 'User', 1, GETDATE(), 1, 0, 1),
('operator1', 'operator1@teklas.com', 'hashedpassword2', 'salt2', 'John', 'Doe', 1, GETDATE(), 1, 0, 1),
('operator2', 'operator2@teklas.com', 'hashedpassword3', 'salt3', 'Jane', 'Smith', 1, GETDATE(), 1, 0, 1),
('supervisor', 'supervisor@teklas.com', 'hashedpassword4', 'salt4', 'Bob', 'Wilson', 1, GETDATE(), 1, 0, 1),
('quality', 'quality@teklas.com', 'hashedpassword5', 'salt5', 'Alice', 'Johnson', 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 2. ROLES (5 records) - Add required fields
-- ===================================================================
INSERT INTO Roles (
    Name, DisplayName, Description, IsSystemRole, IsActive,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('Admin', 'System Administrator', 'System Administrator', 1, 1, GETDATE(), 1, 0, 1),
('ProductionOperator', 'Production Operator', 'Production Line Operator', 0, 1, GETDATE(), 1, 0, 1),
('Supervisor', 'Production Supervisor', 'Production Supervisor', 0, 1, GETDATE(), 1, 0, 1),
('QualityControl', 'Quality Control', 'Quality Control Inspector', 0, 1, GETDATE(), 1, 0, 1),
('Manager', 'Production Manager', 'Production Manager', 0, 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 3. MATERIAL CARDS (5 records) - Use existing category IDs
-- ===================================================================
INSERT INTO MaterialCards (
    CardCode, CardName, CardType, Unit, MaterialCategoryId, 
    PurchasePrice, SalesPrice, CurrentStock, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('LAPTOP-001', 'Gaming Laptop Pro', 'FINISHED_PRODUCT', 'EACH', 1, 800.00, 1200.00, 10, 'High-performance gaming laptop', GETDATE(), 1, 0, 1),
('CPU-001', 'Intel i7 Processor', 'COMPONENT', 'EACH', 2, 200.00, 250.00, 50, 'High-performance processor', GETDATE(), 1, 0, 1),
('GPU-001', 'NVIDIA RTX Graphics', 'COMPONENT', 'EACH', 2, 300.00, 350.00, 25, 'Gaming graphics card', GETDATE(), 1, 0, 1),
('RAM-001', 'DDR4 RAM Module', 'COMPONENT', 'EACH', 2, 5.00, 8.00, 200, 'High-speed memory module', GETDATE(), 1, 0, 1),
('SSD-001', '1TB NVMe SSD', 'COMPONENT', 'EACH', 2, 80.00, 120.00, 100, 'Fast storage device', GETDATE(), 1, 0, 1);

-- ===================================================================
-- 4. USER ROLES (5 records) - Add required fields
-- ===================================================================
INSERT INTO UserRoles (
    UserId, RoleId, AssignedDate, IsActive, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(1, 1, GETDATE(), 1, GETDATE(), 1, 0, 1), -- admin -> Admin
(2, 2, GETDATE(), 1, GETDATE(), 1, 0, 1), -- operator1 -> ProductionOperator
(3, 2, GETDATE(), 1, GETDATE(), 1, 0, 1), -- operator2 -> ProductionOperator
(4, 3, GETDATE(), 1, GETDATE(), 1, 0, 1), -- supervisor -> Supervisor
(5, 4, GETDATE(), 1, GETDATE(), 1, 0, 1); -- quality -> QualityControl

-- ===================================================================
-- 5. BILL OF MATERIALS (3 records) - Use existing material card IDs
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
-- 6. BILL OF MATERIAL ITEMS (6 records) - Add required ScrapFactor
-- ===================================================================
INSERT INTO BillOfMaterialItems (
    BillOfMaterialId, MaterialCardId, LineNumber, Quantity, Unit, ScrapFactor, 
    ComponentType, IssueMethod, Description, IsOptional,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
-- BOM 1 Items
(1, 2, 1, 1.00, 'EACH', 2.0, 'COMPONENT', 'PICK_LIST', 'Processor', 0, GETDATE(), 1, 0, 1),
(1, 3, 2, 1.00, 'EACH', 1.0, 'COMPONENT', 'PICK_LIST', 'Graphics Card', 0, GETDATE(), 1, 0, 1),
(1, 4, 3, 16.00, 'EACH', 1.0, 'COMPONENT', 'PICK_LIST', 'Memory', 0, GETDATE(), 1, 0, 1),
-- BOM 2 Items
(2, 2, 1, 1.00, 'EACH', 2.0, 'COMPONENT', 'PICK_LIST', 'Business Processor', 0, GETDATE(), 1, 0, 1),
(2, 4, 2, 8.00, 'EACH', 1.0, 'COMPONENT', 'PICK_LIST', 'Business Memory', 0, GETDATE(), 1, 0, 1),
(2, 5, 3, 1.00, 'EACH', 1.0, 'COMPONENT', 'PICK_LIST', 'Business Storage', 0, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 7. WORK ORDERS (3 records) - Add required QualityStatus
-- ===================================================================
INSERT INTO WorkOrders (
    WorkOrderNumber, BillOfMaterialId, ProductMaterialCardId, PlannedQuantity, 
    CompletedQuantity, ScrapQuantity, Unit, Status, Priority, QualityStatus,
    PlannedStartDate, PlannedEndDate, Description, WorkCenter,
    WorkOrderType, CompletionPercentage, RequiresQualityCheck,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('WO-2024-001', 1, 1, 10.00, 7.00, 1.00, 'EACH', 'IN_PROGRESS', 1, 'PENDING', DATEADD(day, -2, GETDATE()), DATEADD(day, 3, GETDATE()), 'Gaming laptop production', 'ASSEMBLY_LINE_1', 'PRODUCTION', 80.00, 1, GETDATE(), 1, 0),
('WO-2024-002', 2, 1, 5.00, 0.00, 0.00, 'EACH', 'CREATED', 2, 'NOT_REQUIRED', GETDATE(), DATEADD(day, 2, GETDATE()), 'Business laptop production', 'ASSEMBLY_LINE_2', 'PRODUCTION', 0.00, 0, GETDATE(), 1, 0),
('WO-2024-003', 1, 1, 15.00, 15.00, 2.00, 'EACH', 'COMPLETED', 3, 'PASSED', DATEADD(day, -5, GETDATE()), DATEADD(day, -1, GETDATE()), 'Gaming laptop production', 'ASSEMBLY_LINE_1', 'PRODUCTION', 100.00, 1, GETDATE(), 1, 0);

-- ===================================================================
-- 8. PRODUCTION CONFIRMATIONS (3 records) - Add required QualityStatus
-- ===================================================================
INSERT INTO ProductionConfirmations (
    ConfirmationNumber, WorkOrderId, ConfirmedQuantity, ScrapQuantity, ReworkQuantity,
    Unit, ConfirmationDate, WorkCenter, Status, ConfirmationType, QualityStatus,
    OperatorUserId, Notes, ActivityType, RequiresQualityCheck, MaterialConsumed,
    CreateDate, CreateUserId, IsDeleted
) VALUES
('PC-2024-001', 1, 3.00, 0.00, 0.00, 'EACH', DATEADD(day, -1, GETDATE()), 'ASSEMBLY_LINE_1', 'CONFIRMED', 'PARTIAL', 'PASSED', 2, 'First batch completed', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0),
('PC-2024-002', 1, 4.00, 1.00, 0.00, 'EACH', GETDATE(), 'ASSEMBLY_LINE_1', 'CONFIRMED', 'PARTIAL', 'PASSED', 3, 'Second batch', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0),
('PC-2024-003', 3, 15.00, 2.00, 0.00, 'EACH', DATEADD(day, -1, GETDATE()), 'ASSEMBLY_LINE_1', 'POSTED', 'FINAL', 'PASSED', 2, 'Batch completed', 'ASSEMBLY', 0, 1, GETDATE(), 1, 0);

-- ===================================================================
-- 9. MATERIAL MOVEMENTS (3 records) - Add required MovementDate
-- ===================================================================
INSERT INTO MaterialMovements (
    MaterialCardId, MovementType, Quantity, MovementDate, ReferenceNumber,
    ReferenceType, LocationFrom, LocationTo, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(2, 'IN', 50.00, GETDATE(), '1001', 'PURCHASE_ORDER', 'WAREHOUSE', 'PRODUCTION_LINE_1', 'CPU delivery', GETDATE(), 1, 0, 1),
(3, 'IN', 25.00, GETDATE(), '1002', 'PURCHASE_ORDER', 'WAREHOUSE', 'PRODUCTION_LINE_1', 'GPU delivery', GETDATE(), 1, 0, 1),
(4, 'OUT', 80.00, GETDATE(), '1', 'WORK_ORDER', 'WAREHOUSE', 'ASSEMBLY_LINE_1', 'RAM consumption', GETDATE(), 1, 0, 1);

-- ===================================================================
-- VERIFICATION QUERIES
-- ===================================================================
PRINT '=== MANUAL TEST DATA CREATION COMPLETED ===';
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