-- ===================================================================
-- CORRECTED WORK ORDERS AND PRODUCTION CONFIRMATIONS TEST DATA
-- ===================================================================

-- First, let's check what Material Cards and BOMs exist
SELECT 'Available Material Cards:' as Info;
SELECT Id, CardCode, CardName FROM MaterialCards WHERE IsDeleted = 0;

SELECT 'Available BOMs:' as Info;
SELECT BOMHeaderId, ParentMaterialCardId, Version FROM BOMHeaders WHERE IsDeleted = 0;

-- Get the first available BOMHeaderId to use for Work Orders
DECLARE @FirstBOMHeaderId BIGINT;
SELECT @FirstBOMHeaderId = MIN(BOMHeaderId) FROM BOMHeaders WHERE IsDeleted = 0;

IF @FirstBOMHeaderId IS NULL
BEGIN
    PRINT 'ERROR: No BOMHeaders found. Please create BOMHeaders first.';
    RETURN;
END

PRINT 'Using BOMHeaderId: ' + CAST(@FirstBOMHeaderId AS VARCHAR);

-- ===================================================================
-- 1. WORK ORDERS TEST DATA (using actual table structure)
-- ===================================================================

-- Clear any existing test data first
DELETE FROM MaterialConsumptions WHERE IsDeleted = 0;
DELETE FROM ProductionConfirmations WHERE IsDeleted = 0;
DELETE FROM WorkOrders WHERE IsDeleted = 0;

INSERT INTO WorkOrders (
    BOMHeaderId, MaterialCardId, PlannedQuantity, PlannedStartDate, PlannedEndDate,
    Status, ReferenceNumber, Id, CreateDate, CreateUserId, IsDeleted
) VALUES
-- Work Order 1 - Gaming Laptop Assembly
(@FirstBOMHeaderId, 1, 50.00, GETDATE(), DATEADD(day, 7, GETDATE()), 
 1, 'WO-2024-001', 1, GETDATE(), 1, 0),

-- Work Order 2 - Smartphone Assembly  
(@FirstBOMHeaderId, 2, 100.00, DATEADD(day, 1, GETDATE()), DATEADD(day, 5, GETDATE()),
 1, 'WO-2024-002', 2, GETDATE(), 1, 0),

-- Work Order 3 - Tablet Production
(@FirstBOMHeaderId, 3, 75.00, DATEADD(day, 2, GETDATE()), DATEADD(day, 6, GETDATE()),
 1, 'WO-2024-003', 3, GETDATE(), 1, 0),

-- Work Order 4 - Laptop Repair
(@FirstBOMHeaderId, 1, 25.00, DATEADD(day, 3, GETDATE()), DATEADD(day, 4, GETDATE()),
 1, 'WO-2024-004', 4, GETDATE(), 1, 0),

-- Work Order 5 - Smartphone Repair
(@FirstBOMHeaderId, 2, 30.00, DATEADD(day, 4, GETDATE()), DATEADD(day, 5, GETDATE()),
 1, 'WO-2024-005', 5, GETDATE(), 1, 0);

-- ===================================================================
-- 2. PRODUCTION CONFIRMATIONS TEST DATA (using actual table structure)
-- ===================================================================

-- Get the WorkOrderIds we just created
DECLARE @WO1Id BIGINT, @WO2Id BIGINT, @WO3Id BIGINT, @WO4Id BIGINT, @WO5Id BIGINT;

SELECT @WO1Id = WorkOrderId FROM WorkOrders WHERE ReferenceNumber = 'WO-2024-001' AND IsDeleted = 0;
SELECT @WO2Id = WorkOrderId FROM WorkOrders WHERE ReferenceNumber = 'WO-2024-002' AND IsDeleted = 0;
SELECT @WO3Id = WorkOrderId FROM WorkOrders WHERE ReferenceNumber = 'WO-2024-003' AND IsDeleted = 0;
SELECT @WO4Id = WorkOrderId FROM WorkOrders WHERE ReferenceNumber = 'WO-2024-004' AND IsDeleted = 0;
SELECT @WO5Id = WorkOrderId FROM WorkOrders WHERE ReferenceNumber = 'WO-2024-005' AND IsDeleted = 0;

-- Only insert confirmations if work orders exist
IF @WO1Id IS NOT NULL
BEGIN
    INSERT INTO ProductionConfirmations (
        WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped,
        LaborHoursUsed, PerformedBy, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Confirmation for Work Order 1 (Gaming Laptop Assembly)
    (@WO1Id, GETDATE(), 15.00, 0.50, 12.00, 'John Doe', 1, 1, GETDATE(), 1, 0);
END

IF @WO2Id IS NOT NULL
BEGIN
    INSERT INTO ProductionConfirmations (
        WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped,
        LaborHoursUsed, PerformedBy, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Confirmation for Work Order 2 (Smartphone Assembly)  
    (@WO2Id, DATEADD(day, -1, GETDATE()), 25.00, 1.00, 8.50, 'Jane Smith', 2, 1, GETDATE(), 1, 0);
END

IF @WO3Id IS NOT NULL
BEGIN
    INSERT INTO ProductionConfirmations (
        WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped,
        LaborHoursUsed, PerformedBy, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Confirmation for Work Order 3 (Tablet Production)
    (@WO3Id, DATEADD(day, -2, GETDATE()), 20.00, 0.25, 10.00, 'Mike Johnson', 3, 1, GETDATE(), 1, 0);
END

IF @WO4Id IS NOT NULL
BEGIN
    INSERT INTO ProductionConfirmations (
        WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped,
        LaborHoursUsed, PerformedBy, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Confirmation for Work Order 4 (Laptop Repair)
    (@WO4Id, DATEADD(day, -3, GETDATE()), 8.00, 0.00, 6.00, 'Sarah Wilson', 4, 1, GETDATE(), 1, 0);
END

IF @WO5Id IS NOT NULL
BEGIN
    INSERT INTO ProductionConfirmations (
        WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped,
        LaborHoursUsed, PerformedBy, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Confirmation for Work Order 5 (Smartphone Repair)
    (@WO5Id, DATEADD(day, -4, GETDATE()), 12.00, 0.75, 4.50, 'David Brown', 5, 1, GETDATE(), 1, 0);
END

-- ===================================================================
-- 3. MATERIAL CONSUMPTIONS TEST DATA
-- ===================================================================

DECLARE @PC1Id BIGINT, @PC2Id BIGINT, @PC3Id BIGINT, @PC4Id BIGINT, @PC5Id BIGINT;

-- Get the IDs of the confirmations we just created
SELECT @PC1Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = @WO1Id AND IsDeleted = 0;
SELECT @PC2Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = @WO2Id AND IsDeleted = 0;
SELECT @PC3Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = @WO3Id AND IsDeleted = 0;
SELECT @PC4Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = @WO4Id AND IsDeleted = 0;
SELECT @PC5Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = @WO5Id AND IsDeleted = 0;

-- Insert Material Consumptions for the confirmations (only if confirmations exist)
IF @PC1Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, UnitPrice, TotalAmount,
        BatchNumber, Description, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Material consumption for Gaming Laptop confirmation
    (@PC1Id, 1, 15.00, 1200.00, 18000.00, 'BATCH-2024-001', 'Gaming laptop components', 1, 1, GETDATE(), 1, 0),
    (@PC1Id, 2, 15.00, 150.00, 2250.00, 'BATCH-2024-002', 'Memory modules', 2, 1, GETDATE(), 1, 0),
    (@PC1Id, 3, 15.00, 200.00, 3000.00, 'BATCH-2024-003', 'Storage drives', 3, 1, GETDATE(), 1, 0);
END

IF @PC2Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, UnitPrice, TotalAmount,
        BatchNumber, Description, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Material consumption for Smartphone confirmation
    (@PC2Id, 2, 25.00, 80.00, 2000.00, 'BATCH-2024-004', 'Smartphone components', 4, 1, GETDATE(), 1, 0),
    (@PC2Id, 4, 25.00, 25.00, 625.00, 'BATCH-2024-005', 'Batteries', 5, 1, GETDATE(), 1, 0);
END

IF @PC3Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, UnitPrice, TotalAmount,
        BatchNumber, Description, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Material consumption for Tablet confirmation
    (@PC3Id, 3, 20.00, 300.00, 6000.00, 'BATCH-2024-006', 'Tablet components', 6, 1, GETDATE(), 1, 0),
    (@PC3Id, 5, 20.00, 50.00, 1000.00, 'BATCH-2024-007', 'Screens', 7, 1, GETDATE(), 1, 0);
END

IF @PC4Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, UnitPrice, TotalAmount,
        BatchNumber, Description, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Material consumption for Laptop Repair confirmation
    (@PC4Id, 1, 8.00, 800.00, 6400.00, 'BATCH-2024-008', 'Replacement parts', 8, 1, GETDATE(), 1, 0);
END

IF @PC5Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, UnitPrice, TotalAmount,
        BatchNumber, Description, Id, Status, CreateDate, CreateUserId, IsDeleted
    ) VALUES
    -- Material consumption for Smartphone Repair confirmation
    (@PC5Id, 2, 12.00, 60.00, 720.00, 'BATCH-2024-009', 'Repair components', 9, 1, GETDATE(), 1, 0);
END

-- ===================================================================
-- VERIFICATION QUERIES
-- ===================================================================

SELECT 'Work Orders Created:' as Info;
SELECT 
    WorkOrderId,
    BOMHeaderId,
    MaterialCardId,
    PlannedQuantity,
    PlannedStartDate,
    PlannedEndDate,
    Status,
    ReferenceNumber
FROM WorkOrders 
WHERE IsDeleted = 0 
ORDER BY WorkOrderId;

SELECT 'Production Confirmations Created:' as Info;
SELECT 
    ConfirmationId,
    WorkOrderId,
    ConfirmationDate,
    QuantityProduced,
    QuantityScrapped,
    LaborHoursUsed,
    PerformedBy
FROM ProductionConfirmations 
WHERE IsDeleted = 0 
ORDER BY ConfirmationId;

SELECT 'Material Consumptions Created:' as Info;
SELECT 
    ConsumptionId,
    ConfirmationId,
    MaterialCardId,
    QuantityUsed,
    UnitPrice,
    TotalAmount,
    BatchNumber
FROM MaterialConsumptions 
WHERE IsDeleted = 0 
ORDER BY ConsumptionId; 