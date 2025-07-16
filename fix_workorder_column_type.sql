-- Fix WorkOrder Status column data type from nvarchar to int
-- This script changes the column type to match the enum expected by the entity

-- Step 1: Check current column definition
PRINT '=== CURRENT COLUMN DEFINITION ===';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE,
    CHARACTER_MAXIMUM_LENGTH
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'WorkOrders' AND COLUMN_NAME = 'Status';

-- Step 2: Check current data
PRINT '=== CURRENT DATA ===';
SELECT WorkOrderId, Status, IsDeleted FROM WorkOrders ORDER BY WorkOrderId;

-- Step 3: Update all string values to integer 1 (Active) before changing column type
PRINT '=== UPDATING STRING VALUES TO INTEGERS ===';
UPDATE WorkOrders 
SET Status = 1 
WHERE Status IS NOT NULL 
  AND (ISNUMERIC(Status) = 0 OR Status = '' OR Status = 'Planned' OR Status = 'Active');

-- Handle NULL values
UPDATE WorkOrders 
SET Status = 1 
WHERE Status IS NULL;

-- Step 4: Change column data type from nvarchar to int
PRINT '=== CHANGING COLUMN TYPE ===';
ALTER TABLE WorkOrders ALTER COLUMN Status INT NOT NULL;

-- Step 5: Verify the change
PRINT '=== VERIFICATION ===';
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'WorkOrders' AND COLUMN_NAME = 'Status';

-- Step 6: Show final data
PRINT '=== FINAL DATA ===';
SELECT WorkOrderId, Status, IsDeleted FROM WorkOrders ORDER BY WorkOrderId; 