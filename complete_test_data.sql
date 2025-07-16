-- ===================================================================
-- COMPLETE TEST DATA FOR ALL TABLES - 5 RECORDS EACH
-- ===================================================================

-- Clear existing data in dependency order
DELETE FROM ProductionConfirmations;
DELETE FROM MaterialMovements;
DELETE FROM WorkOrders;
DELETE FROM BOMItems;
DELETE FROM BOMHeaders;
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
    Username, Email, PasswordHash, PasswordSalt, FirstName, LastName, IsActive, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('admin', 'admin@teklas.com', 'hashedpassword1', 'salt1', 'Admin', 'User', 1, GETDATE(), 1, 0, 1),
('operator1', 'operator1@teklas.com', 'hashedpassword2', 'salt2', 'John', 'Doe', 1, GETDATE(), 1, 0, 1),
('operator2', 'operator2@teklas.com', 'hashedpassword3', 'salt3', 'Jane', 'Smith', 1, GETDATE(), 1, 0, 1),
('supervisor', 'supervisor@teklas.com', 'hashedpassword4', 'salt4', 'Bob', 'Wilson', 1, GETDATE(), 1, 0, 1),
('quality', 'quality@teklas.com', 'hashedpassword5', 'salt5', 'Alice', 'Johnson', 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 3. ROLES (5 records)
-- ===================================================================
INSERT INTO Roles (
    Name, DisplayName, Description, IsSystemRole, Priority, IsActive, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('Admin', 'System Administrator', 'Full system access and control', 1, 100, 1, GETDATE(), 1, 0, 1),
('ProductionOperator', 'Production Line Operator', 'Production line operations', 0, 10, 1, GETDATE(), 1, 0, 1),
('Supervisor', 'Production Supervisor', 'Production supervision and management', 0, 50, 1, GETDATE(), 1, 0, 1),
('QualityControl', 'Quality Control Inspector', 'Quality control and inspection', 0, 30, 1, GETDATE(), 1, 0, 1),
('Manager', 'Production Manager', 'Production management and planning', 0, 80, 1, GETDATE(), 1, 0, 1);

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
    UserId, RoleId, AssignedDate, IsActive, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(1, 1, GETDATE(), 1, GETDATE(), 1, 0, 1), -- admin -> Admin
(2, 2, GETDATE(), 1, GETDATE(), 1, 0, 1), -- operator1 -> ProductionOperator
(3, 2, GETDATE(), 1, GETDATE(), 1, 0, 1), -- operator2 -> ProductionOperator
(4, 3, GETDATE(), 1, GETDATE(), 1, 0, 1), -- supervisor -> Supervisor
(5, 4, GETDATE(), 1, GETDATE(), 1, 0, 1); -- quality -> QualityControl

-- ===================================================================
-- 6. BILL OF MATERIALS (5 records)
-- ===================================================================
INSERT INTO BOMHeaders (
    ParentMaterialCardId, Version, ValidFrom, StandardCost, Notes, MaterialCardId, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(1, 'v1.0', GETDATE(), 1200.00, 'Gaming laptop production', 1, GETDATE(), 1, 0, 1),
(1, 'v1.0', GETDATE(), 1000.00, 'Business laptop production', 1, GETDATE(), 1, 0, 1),
(1, 'v1.0', GETDATE(), 800.00, 'Student laptop production', 1, GETDATE(), 1, 0, 1),
(1, 'v2.0', GETDATE(), 1500.00, 'High-end gaming laptop', 1, GETDATE(), 1, 0, 1),
(1, 'v1.0', GETDATE(), 900.00, 'Ultra thin laptop production', 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 7. BILL OF MATERIAL ITEMS (5 records per BOM = 25 total)
-- ===================================================================
INSERT INTO BOMItems (
    BOMHeaderId, ComponentMaterialCardId, Quantity, ScrapRate, MaterialCardId, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
-- BOM 1 Items
(1, 2, 1.00, 2.0, 2, GETDATE(), 1, 0, 1),
(1, 3, 1.00, 1.0, 3, GETDATE(), 1, 0, 1),
(1, 4, 16.00, 1.0, 4, GETDATE(), 1, 0, 1),
(1, 5, 1.00, 1.0, 5, GETDATE(), 1, 0, 1),
(1, 2, 0.50, 5.0, 2, GETDATE(), 1, 0, 1),

-- BOM 2 Items
(2, 2, 1.00, 2.0, 2, GETDATE(), 1, 0, 1),
(2, 4, 8.00, 1.0, 4, GETDATE(), 1, 0, 1),
(2, 5, 1.00, 1.0, 5, GETDATE(), 1, 0, 1),
(2, 3, 0.50, 2.0, 3, GETDATE(), 1, 0, 1),
(2, 2, 0.25, 5.0, 2, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 8. WORK ORDERS (5 records)
-- ===================================================================
INSERT INTO WorkOrders (
    BOMHeaderId, MaterialCardId, PlannedQuantity, PlannedStartDate, PlannedEndDate, Status, ReferenceNumber, CreateDate, CreateUserId, IsDeleted
) VALUES
(1, 1, 10.00, DATEADD(day, -2, GETDATE()), DATEADD(day, 3, GETDATE()), 'IN_PROGRESS', 'WO-2024-001', GETDATE(), 1, 0),
(2, 1, 5.00, GETDATE(), DATEADD(day, 2, GETDATE()), 'CREATED', 'WO-2024-002', GETDATE(), 1, 0),
(1, 1, 15.00, DATEADD(day, -5, GETDATE()), DATEADD(day, -1, GETDATE()), 'COMPLETED', 'WO-2024-003', GETDATE(), 1, 0),
(4, 1, 3.00, DATEADD(day, -1, GETDATE()), DATEADD(day, 4, GETDATE()), 'IN_PROGRESS', 'WO-2024-004', GETDATE(), 1, 0),
(2, 1, 20.00, DATEADD(day, 1, GETDATE()), DATEADD(day, 7, GETDATE()), 'RELEASED', 'WO-2024-005', GETDATE(), 1, 0);

-- ===================================================================
-- 9. PRODUCTION CONFIRMATIONS (5 records)
-- ===================================================================
INSERT INTO ProductionConfirmations (
    WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped, LaborHoursUsed, PerformedBy, MaterialCardId, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(1, DATEADD(day, -1, GETDATE()), 3.00, 0.00, 8.00, 'John Doe', 1, GETDATE(), 1, 0, 1),
(1, GETDATE(), 4.00, 1.00, 10.00, 'Jane Smith', 1, GETDATE(), 1, 0, 1),
(3, DATEADD(day, -1, GETDATE()), 15.00, 2.00, 40.00, 'Bob Wilson', 1, GETDATE(), 1, 0, 1),
(4, GETDATE(), 1.00, 0.00, 3.00, 'Alice Johnson', 1, GETDATE(), 1, 0, 1),
(1, GETDATE(), 0.00, 0.00, 2.00, 'John Doe', 1, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 10. MATERIAL MOVEMENTS (5 records)
-- ===================================================================
INSERT INTO MaterialMovements (
    MaterialCardId, MovementType, Quantity, UnitPrice, TotalAmount, MovementDate, ReferenceNumber, ReferenceType, LocationFrom, LocationTo, Description, CreateDate, CreateUserId, IsDeleted, Status
) VALUES
(2, 'IN', 50.00, 200.00, 10000.00, GETDATE(), 'PO-1001', 'PURCHASE_ORDER', 'WAREHOUSE', 'PRODUCTION_LINE_1', 'CPU delivery for production', GETDATE(), 1, 0, 1),
(3, 'IN', 25.00, 300.00, 7500.00, GETDATE(), 'PO-1002', 'PURCHASE_ORDER', 'WAREHOUSE', 'PRODUCTION_LINE_1', 'GPU delivery for production', GETDATE(), 1, 0, 1),
(4, 'OUT', 80.00, 5.00, 400.00, GETDATE(), 'WO-2024-001', 'WORK_ORDER', 'WAREHOUSE', 'ASSEMBLY_LINE_1', 'RAM consumption for WO-2024-001', GETDATE(), 1, 0, 1),
(5, 'OUT', 10.00, 80.00, 800.00, GETDATE(), 'WO-2024-001', 'WORK_ORDER', 'WAREHOUSE', 'ASSEMBLY_LINE_1', 'SSD consumption for WO-2024-001', GETDATE(), 1, 0, 1),
(1, 'OUT', 7.00, 1200.00, 8400.00, GETDATE(), 'SO-2001', 'SALES_ORDER', 'FINISHED_GOODS', 'SHIPPING', 'Laptop shipment to customer', GETDATE(), 1, 0, 1);

-- ===================================================================
-- VERIFICATION QUERIES
-- ===================================================================
PRINT '=== TEST DATA CREATION COMPLETED ===';
SELECT 'MaterialCategories' as TableName, COUNT(*) as RecordCount FROM MaterialCategories
UNION ALL SELECT 'Users', COUNT(*) FROM Users
UNION ALL SELECT 'Roles', COUNT(*) FROM Roles
UNION ALL SELECT 'MaterialCards', COUNT(*) FROM MaterialCards
UNION ALL SELECT 'UserRoles', COUNT(*) FROM UserRoles
UNION ALL SELECT 'BOMHeaders', COUNT(*) FROM BOMHeaders
UNION ALL SELECT 'BOMItems', COUNT(*) FROM BOMItems
UNION ALL SELECT 'WorkOrders', COUNT(*) FROM WorkOrders
UNION ALL SELECT 'ProductionConfirmations', COUNT(*) FROM ProductionConfirmations
UNION ALL SELECT 'MaterialMovements', COUNT(*) FROM MaterialMovements
ORDER BY TableName;

PRINT '=== SAMPLE DATA OVERVIEW ===';
PRINT 'Work Orders Status Distribution:';
SELECT Status, COUNT(*) as Count FROM WorkOrders GROUP BY Status;

PRINT 'Production Summary:';
SELECT 
    wo.ReferenceNumber,
    wo.PlannedQuantity,
    wo.Status,
    bom.Version
FROM WorkOrders wo
JOIN BOMHeaders bom ON wo.BOMHeaderId = bom.BOMHeaderId
ORDER BY wo.ReferenceNumber; 