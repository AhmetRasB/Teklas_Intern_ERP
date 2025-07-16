-- Fix WorkOrder Status column data type mismatch
-- This script updates existing string values in the Status column to proper enum integers

-- First, let's see what values we currently have
SELECT WorkOrderId, Status, IsDeleted FROM WorkOrders;

-- Update all WorkOrder records to have Status = 1 (Active) instead of string values
UPDATE WorkOrders 
SET Status = 1 
WHERE Status IS NOT NULL;

-- Verify the fix
SELECT WorkOrderId, Status, IsDeleted FROM WorkOrders;

-- If you want to be more specific and handle different status values:
-- UPDATE WorkOrders SET Status = 1 WHERE Status = 'Planned' OR Status = 'Active';
-- UPDATE WorkOrders SET Status = 2 WHERE Status = 'Passive';
-- UPDATE WorkOrders SET Status = 3 WHERE Status = 'Blocked';
-- UPDATE WorkOrders SET Status = 4 WHERE Status = 'Frozen'; 