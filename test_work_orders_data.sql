-- ===================================================================
-- WORK ORDERS TEST DATA FOR PRODUCTION CONFIRMATIONS
-- ===================================================================

-- First, let's check what Material Cards exist for BOMs
SELECT 'Available Material Cards for BOMs:' as Info;
SELECT MaterialCardId, MaterialCode, MaterialName FROM MaterialCards WHERE IsDeleted = 0;

-- Insert Work Orders
INSERT INTO WorkOrders (
    WorkOrderNumber, BOMHeaderId, MaterialCardId, PlannedQuantity, CompletedQuantity, ScrappedQuantity,
    Unit, Status, Priority, PlannedStartDate, PlannedEndDate, ActualStartDate, ActualEndDate,
    WorkCenter, Description, SalesOrderNumber, WorkCenterCode, Shift, ResponsibleUserId,
    StandardHours, ActualHours, SetupTime, RunTime, ActivityType, OrderType, CustomerOrderId,
    BatchNumber, SerialNumber, CostCenter, MaterialConsumed, QualityCheckRequired, QualityCheckResult,
    CreateDate, CreateUserId, IsDeleted
) VALUES
-- Work Order 1 - Gaming Laptop Assembly
('WO-2024-001', 1, 1, 50.00, 0.00, 0.00, 'EACH', 1, 1, 
 GETDATE(), DATEADD(day, 7, GETDATE()), NULL, NULL, 
 'ASSEMBLY_LINE_1', 'Gaming laptop assembly for Q1 production', 'SO-2024-1001', 'ASSEMBLY_LINE_1', 'DAY_SHIFT', 1,
 40.00, 0.00, 2.00, 38.00, 'PRODUCTION', 'SALES_ORDER', 1001, NULL, NULL,
 0.00, 1, 'NOT_REQUIRED', GETDATE(), 1, 0),

-- Work Order 2 - Smartphone Assembly  
('WO-2024-002', 2, 2, 100.00, 0.00, 0.00, 'EACH', 1, 2,
 DATEADD(day, 1, GETDATE()), DATEADD(day, 5, GETDATE()), NULL, NULL,
 'ASSEMBLY_LINE_2', 'Smartphone assembly for retail orders', 'SO-2024-1002', 'ASSEMBLY_LINE_2', 'DAY_SHIFT', 1,
 25.00, 0.00, 1.50, 23.50, 'PRODUCTION', 'SALES_ORDER', 1002, NULL, NULL,
 0.00, 1, 'NOT_REQUIRED', GETDATE(), 1, 0),

-- Work Order 3 - Tablet Production
('WO-2024-003', 3, 3, 75.00, 0.00, 0.00, 'EACH', 1, 1,
 DATEADD(day, 2, GETDATE()), DATEADD(day, 6, GETDATE()), NULL, NULL,
 'ASSEMBLY_LINE_1', 'Tablet production for enterprise customers', 'SO-2024-1003', 'ASSEMBLY_LINE_1', 'DAY_SHIFT', 1,
 30.00, 0.00, 1.00, 29.00, 'PRODUCTION', 'SALES_ORDER', 1003, NULL, NULL,
 0.00, 1, 'NOT_REQUIRED', GETDATE(), 1, 0),

-- Work Order 4 - Laptop Repair
('WO-2024-004', 1, 1, 25.00, 0.00, 0.00, 'EACH', 1, 1,
 DATEADD(day, 1, GETDATE()), DATEADD(day, 4, GETDATE()), NULL, NULL,
 'REPAIR_STATION', 'Laptop repair and refurbishment', 'SO-2024-1004', 'REPAIR_STATION', 'DAY_SHIFT', 1,
 20.00, 0.00, 0.50, 19.50, 'REPAIR', 'SALES_ORDER', 1004, NULL, NULL,
 0.00, 1, 'REQUIRED', GETDATE(), 1, 0),

-- Work Order 5 - Smartphone Repair
('WO-2024-005', 2, 2, 40.00, 0.00, 0.00, 'EACH', 1, 2,
 DATEADD(day, 3, GETDATE()), DATEADD(day, 8, GETDATE()), NULL, NULL,
 'REPAIR_STATION', 'Smartphone repair and testing', 'SO-2024-1005', 'REPAIR_STATION', 'DAY_SHIFT', 1,
 15.00, 0.00, 0.25, 14.75, 'REPAIR', 'SALES_ORDER', 1005, NULL, NULL,
 0.00, 1, 'REQUIRED', GETDATE(), 1, 0);

-- Show the created Work Orders
SELECT 'Work Orders created:' as Info;
SELECT 
    wo.WorkOrderId,
    wo.WorkOrderNumber,
    wo.BOMHeaderId,
    wo.MaterialCardId,
    mc.MaterialName as ProductName,
    wo.PlannedQuantity,
    wo.Status,
    wo.WorkCenter,
    wo.Description,
    wo.PlannedStartDate,
    wo.PlannedEndDate
FROM WorkOrders wo
LEFT JOIN MaterialCards mc ON wo.MaterialCardId = mc.MaterialCardId
WHERE wo.IsDeleted = 0
ORDER BY wo.WorkOrderId; 