-- ===================================================================
-- BASIC TEST DATA - START FROM SCRATCH
-- ===================================================================

-- 1. Create Material Categories first
INSERT INTO MaterialCategories (
    CategoryCode, CategoryName, Description, ParentCategoryId,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('CAT-001', 'Electronics', 'Electronic products and components', NULL, GETDATE(), 1, 0, 1),
('CAT-002', 'Components', 'Raw components for assembly', NULL, GETDATE(), 1, 0, 1);

-- 2. Create Material Cards
INSERT INTO MaterialCards (
    CardCode, CardName, CardType, Unit, MaterialCategoryId, 
    PurchasePrice, SalesPrice, CurrentStock, Description,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
-- Finished Products
('LAPTOP-001', 'Gaming Laptop', 'FINISHED_PRODUCT', 'EACH', 1, 800.00, 1200.00, 0, 'Gaming laptop', GETDATE(), 1, 0, 1),
-- Components  
('CPU-001', 'Processor', 'COMPONENT', 'EACH', 2, 200.00, 250.00, 100, 'CPU', GETDATE(), 1, 0, 1),
('GPU-001', 'Graphics Card', 'COMPONENT', 'EACH', 2, 300.00, 350.00, 50, 'GPU', GETDATE(), 1, 0, 1);

-- 3. Create BOM (need to check required fields first)
INSERT INTO BillOfMaterials (
    BOMCode, BOMName, Version, ProductMaterialCardId, BaseQuantity, Unit,
    BOMType, Description, ApprovalStatus, IsActive,
    CreateDate, CreateUserId, IsDeleted, Status
) VALUES
('BOM-001', 'Laptop Assembly', 'v1.0', 1, 1.00, 'EACH',
 'ASSEMBLY', 'Basic laptop assembly', 'APPROVED', 1,
 GETDATE(), 1, 0, 1);

-- Check what we created
SELECT 'Material Categories: ' + CAST(COUNT(*) AS VARCHAR) FROM MaterialCategories;
SELECT 'Material Cards: ' + CAST(COUNT(*) AS VARCHAR) FROM MaterialCards;
SELECT 'BOMs: ' + CAST(COUNT(*) AS VARCHAR) FROM BillOfMaterials; 