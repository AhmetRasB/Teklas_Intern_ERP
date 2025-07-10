-- ===================================================================
-- PRODUCTION MANAGEMENT TEST DATA
-- Teklas Intern ERP - Production Module
-- ===================================================================

-- Clean existing data (in reverse order of dependencies)
DELETE FROM ProductionConfirmations;
DELETE FROM WorkOrders;
DELETE FROM BillOfMaterialItems;
DELETE FROM BillOfMaterials;

-- ===================================================================
-- 1. BILL OF MATERIALS (BOM) TEST DATA
-- ===================================================================

-- BOM for Laptop Assembly
INSERT INTO BillOfMaterials (
    BOMCode, BOMName, Version, ProductMaterialCardId, BaseQuantity, Unit,
    BOMType, Description, RouteCode, StandardTime, SetupTime, ApprovalStatus,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('BOM-LAPTOP-001', 'Gaming Laptop Assembly', 'v1.0', 1, 1.00, 'EACH',
 'ASSEMBLY', 'High-performance gaming laptop assembly process', 'ROUTE-001', 120.00, 30.00, 'APPROVED',
 GETDATE(), 1, 0, 1),

('BOM-PHONE-001', 'Smartphone Assembly', 'v2.1', 2, 1.00, 'EACH',
 'ASSEMBLY', 'Flagship smartphone assembly with advanced features', 'ROUTE-002', 45.00, 15.00, 'APPROVED',
 GETDATE(), 1, 0, 1),

('BOM-TABLET-001', 'Tablet Production', 'v1.5', 3, 1.00, 'EACH',
 'PRODUCTION', 'Professional tablet manufacturing process', 'ROUTE-003', 75.00, 20.00, 'APPROVED',
 GETDATE(), 1, 0, 1);

-- ===================================================================
-- 2. BILL OF MATERIAL ITEMS TEST DATA
-- ===================================================================

-- BOM Items for Gaming Laptop (BOM ID: 1)
INSERT INTO BillOfMaterialItems (
    BillOfMaterialId, MaterialCardId, Quantity, Unit, ScrapFactor, 
    ComponentType, IssueMethod, Description, CostAllocation,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
-- Core Components
(1, 4, 1.00, 'EACH', 0.02, 'COMPONENT', 'PICK_LIST', 'Intel i7 Processor', 25.00, GETDATE(), 1, 0, 1),
(1, 5, 1.00, 'EACH', 0.01, 'COMPONENT', 'PICK_LIST', 'NVIDIA RTX Graphics Card', 35.00, GETDATE(), 1, 0, 1),
(1, 6, 16.00, 'GB', 0.01, 'COMPONENT', 'PICK_LIST', 'DDR4 RAM Module', 10.00, GETDATE(), 1, 0, 1),
(1, 7, 1.00, 'EACH', 0.01, 'COMPONENT', 'PICK_LIST', '1TB NVMe SSD', 8.00, GETDATE(), 1, 0, 1),
(1, 8, 1.00, 'EACH', 0.05, 'COMPONENT', 'PICK_LIST', 'Laptop Battery', 5.00, GETDATE(), 1, 0, 1),

-- BOM Items for Smartphone (BOM ID: 2)
(2, 4, 1.00, 'EACH', 0.01, 'COMPONENT', 'PICK_LIST', 'Mobile Processor', 20.00, GETDATE(), 1, 0, 1),
(2, 9, 1.00, 'EACH', 0.03, 'COMPONENT', 'PICK_LIST', 'OLED Display', 25.00, GETDATE(), 1, 0, 1),
(2, 10, 1.00, 'EACH', 0.02, 'COMPONENT', 'PICK_LIST', 'Camera Module', 15.00, GETDATE(), 1, 0, 1),
(2, 8, 1.00, 'EACH', 0.04, 'COMPONENT', 'PICK_LIST', 'Phone Battery', 8.00, GETDATE(), 1, 0, 1),

-- BOM Items for Tablet (BOM ID: 3)
(3, 4, 1.00, 'EACH', 0.02, 'COMPONENT', 'PICK_LIST', 'Tablet Processor', 18.00, GETDATE(), 1, 0, 1),
(3, 9, 1.00, 'EACH', 0.03, 'COMPONENT', 'PICK_LIST', 'Tablet Display', 22.00, GETDATE(), 1, 0, 1),
(3, 8, 1.00, 'EACH', 0.03, 'COMPONENT', 'PICK_LIST', 'Tablet Battery', 12.00, GETDATE(), 1, 0, 1);

-- ===================================================================
-- 3. WORK ORDERS TEST DATA
-- ===================================================================

INSERT INTO WorkOrders (
    WorkOrderNumber, BillOfMaterialId, ProductMaterialCardId, PlannedQuantity, 
    CompletedQuantity, ScrapQuantity, Unit, Status, Priority, 
    PlannedStartDate, PlannedEndDate, ActualStartDate, ActualEndDate, DueDate,
    Description, CustomerOrderReference, WorkCenter, Shift, SupervisorUserId,
    PlannedSetupTime, PlannedRunTime, ActualSetupTime, ActualRunTime,
    WorkOrderType, SourceType, SourceReferenceId, ReleasedDate, ReleasedByUserId,
    CompletionPercentage, RequiresQualityCheck, QualityStatus,
    CreateDate, CreateUserId, IsDeleted
) VALUES
-- Active Work Orders
('WO-2024-001', 1, 1, 50.00, 25.00, 2.00, 'EACH', 'IN_PROGRESS', 1,
 DATEADD(day, -2, GETDATE()), DATEADD(day, 3, GETDATE()), DATEADD(day, -2, GETDATE()), NULL, DATEADD(day, 3, GETDATE()),
 'Gaming laptop production batch for Q4 orders', 'SO-2024-1001', 'ASSEMBLY_LINE_1', 'DAY_SHIFT', 1,
 30.00, 120.00, 28.00, 95.00, 'PRODUCTION', 'SALES_ORDER', 1001, DATEADD(day, -2, GETDATE()), 1,
 54.00, 1, 'PENDING', GETDATE(), 1, 0),

('WO-2024-002', 2, 2, 100.00, 0.00, 0.00, 'EACH', 'RELEASED', 2,
 GETDATE(), DATEADD(day, 2, GETDATE()), NULL, NULL, DATEADD(day, 2, GETDATE()),
 'Smartphone production for new model launch', 'SO-2024-1002', 'ASSEMBLY_LINE_2', 'DAY_SHIFT', 1,
 15.00, 45.00, NULL, NULL, 'PRODUCTION', 'SALES_ORDER', 1002, GETDATE(), 1,
 0.00, 1, 'NOT_REQUIRED', GETDATE(), 1, 0),

('WO-2024-003', 3, 3, 75.00, 75.00, 3.00, 'EACH', 'COMPLETED', 3,
 DATEADD(day, -5, GETDATE()), DATEADD(day, -1, GETDATE()), DATEADD(day, -5, GETDATE()), DATEADD(day, -1, GETDATE()), DATEADD(day, -1, GETDATE()),
 'Tablet production for corporate orders', 'SO-2024-1003', 'ASSEMBLY_LINE_3', 'NIGHT_SHIFT', 1,
 20.00, 75.00, 22.00, 78.00, 'PRODUCTION', 'SALES_ORDER', 1003, DATEADD(day, -5, GETDATE()), 1,
 100.00, 1, 'PASSED', GETDATE(), 1, 0),

-- Planned Work Orders
('WO-2024-004', 1, 1, 25.00, 0.00, 0.00, 'EACH', 'CREATED', 1,
 DATEADD(day, 1, GETDATE()), DATEADD(day, 4, GETDATE()), NULL, NULL, DATEADD(day, 4, GETDATE()),
 'Gaming laptop rush order for premium customers', 'SO-2024-1004', 'ASSEMBLY_LINE_1', 'DAY_SHIFT', 1,
 30.00, 120.00, NULL, NULL, 'PRODUCTION', 'SALES_ORDER', 1004, NULL, NULL,
 0.00, 1, 'NOT_REQUIRED', GETDATE(), 1, 0),

('WO-2024-005', 2, 2, 200.00, 0.00, 0.00, 'EACH', 'CREATED', 2,
 DATEADD(day, 3, GETDATE()), DATEADD(day, 8, GETDATE()), NULL, NULL, DATEADD(day, 8, GETDATE()),
 'Large smartphone order for retail chain', 'SO-2024-1005', 'ASSEMBLY_LINE_2', 'DAY_SHIFT', 1,
 15.00, 45.00, NULL, NULL, 'PRODUCTION', 'SALES_ORDER', 1005, NULL, NULL,
 0.00, 1, 'NOT_REQUIRED', GETDATE(), 1, 0);

-- ===================================================================
-- 4. PRODUCTION CONFIRMATIONS TEST DATA
-- ===================================================================

INSERT INTO ProductionConfirmations (
    ConfirmationNumber, WorkOrderId, ConfirmedQuantity, ScrapQuantity, ReworkQuantity,
    Unit, ConfirmationDate, WorkCenter, Status, ConfirmationType,
    OperatorUserId, SetupTime, RunTime, DownTime, DownTimeReason, Shift,
    Notes, QualityStatus, QualityNotes, BatchNumber, SerialNumberFrom, SerialNumberTo,
    CostCenter, MaterialConsumed, RequiresQualityCheck, QualityCheckResult,
    ActivityType, StartTime, EndTime, WaitTime, PostedByUserId, PostedDate,
    CreateDate, CreateUserId, IsDeleted
) VALUES
-- Confirmations for WO-2024-001 (Gaming Laptop - In Progress)
('PC-2024-001', 1, 15.00, 1.00, 0.00, 'EACH', DATEADD(day, -1, GETDATE()), 'ASSEMBLY_LINE_1', 'POSTED', 'PARTIAL',
 1, 28.00, 45.00, 5.00, 'Equipment calibration', 'DAY_SHIFT',
 'First batch completed successfully', 'PASSED', 'Quality check passed with minor adjustments', 'BATCH-001', 'SN001001', 'SN001015',
 'CC-ASSEMBLY', 1, 1, 'PASSED', 'ASSEMBLY', DATEADD(hour, -10, GETDATE()), DATEADD(hour, -6, GETDATE()), 2.00, 1, DATEADD(day, -1, GETDATE()),
 DATEADD(day, -1, GETDATE()), 1, 0),

('PC-2024-002', 1, 10.00, 1.00, 0.00, 'EACH', GETDATE(), 'ASSEMBLY_LINE_1', 'CONFIRMED', 'PARTIAL',
 1, 25.00, 40.00, 3.00, 'Material shortage delay', 'DAY_SHIFT',
 'Second batch in progress', 'PENDING', 'Awaiting final quality inspection', 'BATCH-002', 'SN001016', 'SN001025',
 'CC-ASSEMBLY', 1, 1, 'PENDING', 'ASSEMBLY', DATEADD(hour, -6, GETDATE()), DATEADD(hour, -2, GETDATE()), 1.00, NULL, NULL,
 GETDATE(), 1, 0),

-- Confirmations for WO-2024-003 (Tablet - Completed)
('PC-2024-003', 3, 25.00, 1.00, 0.00, 'EACH', DATEADD(day, -3, GETDATE()), 'ASSEMBLY_LINE_3', 'POSTED', 'PARTIAL',
 1, 22.00, 26.00, 2.00, 'Shift change delay', 'NIGHT_SHIFT',
 'First tablet batch completed', 'PASSED', 'Excellent quality standards met', 'BATCH-TAB-001', 'TB001001', 'TB001025',
 'CC-ASSEMBLY', 1, 1, 'PASSED', 'ASSEMBLY', DATEADD(day, -3, DATEADD(hour, -8, GETDATE())), DATEADD(day, -3, DATEADD(hour, -4, GETDATE())), 1.50, 1, DATEADD(day, -3, GETDATE()),
 DATEADD(day, -3, GETDATE()), 1, 0),

('PC-2024-004', 3, 25.00, 1.00, 0.00, 'EACH', DATEADD(day, -2, GETDATE()), 'ASSEMBLY_LINE_3', 'POSTED', 'PARTIAL',
 1, 20.00, 25.00, 1.00, 'None', 'NIGHT_SHIFT',
 'Second tablet batch completed efficiently', 'PASSED', 'Perfect quality results', 'BATCH-TAB-002', 'TB001026', 'TB001050',
 'CC-ASSEMBLY', 1, 1, 'PASSED', 'ASSEMBLY', DATEADD(day, -2, DATEADD(hour, -8, GETDATE())), DATEADD(day, -2, DATEADD(hour, -4, GETDATE())), 0.50, 1, DATEADD(day, -2, GETDATE()),
 DATEADD(day, -2, GETDATE()), 1, 0),

('PC-2024-005', 3, 25.00, 1.00, 0.00, 'EACH', DATEADD(day, -1, GETDATE()), 'ASSEMBLY_LINE_3', 'POSTED', 'FINAL',
 1, 18.00, 27.00, 0.00, 'None', 'NIGHT_SHIFT',
 'Final tablet batch - order completed', 'PASSED', 'Outstanding quality achievement', 'BATCH-TAB-003', 'TB001051', 'TB001075',
 'CC-ASSEMBLY', 1, 1, 'PASSED', 'ASSEMBLY', DATEADD(day, -1, DATEADD(hour, -8, GETDATE())), DATEADD(day, -1, DATEADD(hour, -4, GETDATE())), 0.00, 1, DATEADD(day, -1, GETDATE()),
 DATEADD(day, -1, GETDATE()), 1, 0);

-- ===================================================================
-- VERIFICATION QUERIES
-- ===================================================================

-- Summary Information
PRINT '=== PRODUCTION MANAGEMENT TEST DATA SUMMARY ===';
PRINT 'Bill of Materials: ' + CAST((SELECT COUNT(*) FROM BillOfMaterials) AS VARCHAR);
PRINT 'BOM Items: ' + CAST((SELECT COUNT(*) FROM BillOfMaterialItems) AS VARCHAR);
PRINT 'Work Orders: ' + CAST((SELECT COUNT(*) FROM WorkOrders) AS VARCHAR);
PRINT 'Production Confirmations: ' + CAST((SELECT COUNT(*) FROM ProductionConfirmations) AS VARCHAR);
PRINT '';
PRINT 'Work Order Status Distribution:';
SELECT Status, COUNT(*) as Count FROM WorkOrders GROUP BY Status;
PRINT '';
PRINT 'Production Summary by Work Order:';
SELECT 
    wo.WorkOrderNumber,
    wo.PlannedQuantity,
    wo.CompletedQuantity,
    wo.ScrapQuantity,
    wo.CompletionPercentage,
    wo.Status
FROM WorkOrders wo
ORDER BY wo.WorkOrderNumber; 