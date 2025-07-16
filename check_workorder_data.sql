-- Check current WorkOrder data to identify the casting issue
-- Run this first to see what's in your database

-- Check the data types and values in WorkOrders table
SELECT 
    WorkOrderId,
    Status,
    SQL_VARIANT_PROPERTY(Status, 'BaseType') as StatusDataType,
    IsDeleted,
    CreateDate
FROM WorkOrders
ORDER BY WorkOrderId;

-- Check if there are any non-numeric values in Status column
SELECT 
    WorkOrderId,
    Status,
    CASE 
        WHEN ISNUMERIC(Status) = 1 THEN 'Numeric'
        ELSE 'Non-Numeric'
    END as StatusType
FROM WorkOrders
WHERE Status IS NOT NULL
ORDER BY WorkOrderId;

-- Show all columns to understand the full structure
SELECT TOP 5 * FROM WorkOrders; 