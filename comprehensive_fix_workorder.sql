-- Comprehensive fix for WorkOrder Status column casting issue
-- This script handles all possible string values and converts them to proper integers

-- Step 1: Check current data
PRINT '=== CURRENT DATA ===';
SELECT WorkOrderId, Status, IsDeleted FROM WorkOrders ORDER BY WorkOrderId;

-- Step 2: Update all string values to integer 1 (Active)
PRINT '=== UPDATING STATUS VALUES ===';

-- Handle any string values (including 'Planned', 'Active', etc.)
UPDATE WorkOrders 
SET Status = 1 
WHERE Status IS NOT NULL 
  AND (ISNUMERIC(Status) = 0 OR Status = '');

-- Also handle any NULL values
UPDATE WorkOrders 
SET Status = 1 
WHERE Status IS NULL;

-- Step 3: Verify the fix
PRINT '=== VERIFICATION ===';
SELECT 
    WorkOrderId,
    Status,
    CASE 
        WHEN ISNUMERIC(Status) = 1 THEN 'Numeric - OK'
        ELSE 'Still String - PROBLEM'
    END as StatusCheck,
    IsDeleted
FROM WorkOrders 
ORDER BY WorkOrderId;

-- Step 4: Show final result
PRINT '=== FINAL RESULT ===';
SELECT WorkOrderId, Status, IsDeleted FROM WorkOrders ORDER BY WorkOrderId; 