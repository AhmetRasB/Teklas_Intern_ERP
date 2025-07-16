-- ===================================================================
-- PRODUCTION CONFIRMATIONS TEST DATA
-- ===================================================================

-- First, let's check what Work Orders exist
SELECT 'Available Work Orders:' as Info;
SELECT WorkOrderId, WorkOrderNumber, Status FROM WorkOrders WHERE IsDeleted = 0;

-- Insert Production Confirmations for the Work Orders
INSERT INTO ProductionConfirmations (
    WorkOrderId, ConfirmationDate, QuantityProduced, QuantityScrapped,
    LaborHoursUsed, PerformedBy, Status, CreateDate, CreateUserId, IsDeleted
) VALUES
-- Confirmation for Work Order 1 (Gaming Laptop Assembly)
(1, GETDATE(), 15.00, 0.50, 12.00, 'John Doe', 3, GETDATE(), 1, 0),

-- Confirmation for Work Order 2 (Smartphone Assembly)  
(2, DATEADD(day, -1, GETDATE()), 25.00, 1.00, 8.50, 'Jane Smith', 3, GETDATE(), 1, 0),

-- Confirmation for Work Order 3 (Tablet Production)
(3, DATEADD(day, -2, GETDATE()), 20.00, 0.25, 10.00, 'Mike Johnson', 3, GETDATE(), 1, 0),

-- Confirmation for Work Order 4 (Laptop Repair)
(4, DATEADD(day, -3, GETDATE()), 8.00, 0.00, 6.00, 'Sarah Wilson', 3, GETDATE(), 1, 0),

-- Confirmation for Work Order 5 (Smartphone Repair)
(5, DATEADD(day, -4, GETDATE()), 12.00, 0.75, 4.50, 'David Brown', 3, GETDATE(), 1, 0);

-- Insert Material Consumptions for the confirmations
DECLARE @PC1Id BIGINT, @PC2Id BIGINT, @PC3Id BIGINT, @PC4Id BIGINT, @PC5Id BIGINT;

-- Get the IDs of the confirmations we just created
SELECT @PC1Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = 1 AND IsDeleted = 0;
SELECT @PC2Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = 2 AND IsDeleted = 0;
SELECT @PC3Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = 3 AND IsDeleted = 0;
SELECT @PC4Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = 4 AND IsDeleted = 0;
SELECT @PC5Id = ConfirmationId FROM ProductionConfirmations WHERE WorkOrderId = 5 AND IsDeleted = 0;

-- Insert consumptions if confirmations exist
IF @PC1Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, BatchNumber, Status,
        CreateDate, CreateUserId, IsDeleted
    ) VALUES
    (@PC1Id, 1, 15.00, 'BATCH-PC-001', 4, GETDATE(), 1, 0),
    (@PC1Id, 2, 15.00, 'BATCH-PC-002', 4, GETDATE(), 1, 0);
END

IF @PC2Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, BatchNumber, Status,
        CreateDate, CreateUserId, IsDeleted
    ) VALUES
    (@PC2Id, 1, 25.00, 'BATCH-PC-003', 4, GETDATE(), 1, 0),
    (@PC2Id, 3, 25.00, 'BATCH-PC-004', 4, GETDATE(), 1, 0);
END

IF @PC3Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, BatchNumber, Status,
        CreateDate, CreateUserId, IsDeleted
    ) VALUES
    (@PC3Id, 2, 20.00, 'BATCH-PC-005', 4, GETDATE(), 1, 0);
END

IF @PC4Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, BatchNumber, Status,
        CreateDate, CreateUserId, IsDeleted
    ) VALUES
    (@PC4Id, 1, 8.00, 'BATCH-PC-006', 4, GETDATE(), 1, 0);
END

IF @PC5Id IS NOT NULL
BEGIN
    INSERT INTO MaterialConsumptions (
        ConfirmationId, MaterialCardId, QuantityUsed, BatchNumber, Status,
        CreateDate, CreateUserId, IsDeleted
    ) VALUES
    (@PC5Id, 1, 12.00, 'BATCH-PC-007', 4, GETDATE(), 1, 0);
END

-- Show the results
SELECT 'Production Confirmations created:' as Info;
SELECT 
    pc.ConfirmationId,
    pc.WorkOrderId,
    wo.WorkOrderNumber,
    pc.ConfirmationDate,
    pc.QuantityProduced,
    pc.QuantityScrapped,
    pc.LaborHoursUsed,
    pc.PerformedBy,
    COUNT(mc.ConsumptionId) as MaterialConsumptions
FROM ProductionConfirmations pc
LEFT JOIN WorkOrders wo ON pc.WorkOrderId = wo.WorkOrderId
LEFT JOIN MaterialConsumptions mc ON pc.ConfirmationId = mc.ConfirmationId AND mc.IsDeleted = 0
WHERE pc.IsDeleted = 0
GROUP BY pc.ConfirmationId, pc.WorkOrderId, wo.WorkOrderNumber, pc.ConfirmationDate, pc.QuantityProduced, 
         pc.QuantityScrapped, pc.LaborHoursUsed, pc.PerformedBy
ORDER BY pc.ConfirmationId; 