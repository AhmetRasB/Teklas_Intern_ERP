using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.Entities.SalesOrderManagement;

namespace Teklas_Intern_ERP.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MaterialCard> MaterialCards { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<MaterialMovement> MaterialMovements { get; set; }
        public DbSet<BillOfMaterials> BillOfMaterials { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<ProductionConfirmation> ProductionConfirmations { get; set; }
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<StockEntry> StockEntries { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<SupplierType> SupplierTypes { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<MaterialBatch> MaterialBatches { get; set; }
    }
} 