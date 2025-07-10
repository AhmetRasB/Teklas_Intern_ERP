-- Add one basic material card for API testing
INSERT INTO MaterialCards (
    CardCode, CardName, CardType, Unit, MaterialCategoryId, 
    PurchasePrice, SalesPrice, CurrentStock, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES (
    'LAPTOP-TEST', 'Test Gaming Laptop', 'FINISHED_PRODUCT', 'EACH', 4, 
    800.00, 1200.00, 10, 'Test laptop for API testing',
    GETDATE(), 1, 0, 1
);

-- Verify creation
SELECT 'Material Card Created:' AS Result, CardCode, CardName FROM MaterialCards WHERE CardCode = 'LAPTOP-TEST'; 