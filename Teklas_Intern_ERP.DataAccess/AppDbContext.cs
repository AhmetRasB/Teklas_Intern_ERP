using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.Entities.ProductionManagment;
using Teklas_Intern_ERP.Entities.PurchasingManagement;
using Teklas_Intern_ERP.Entities.SalesManagement;
using Teklas_Intern_ERP.Entities.UserManagement;
using Teklas_Intern_ERP.Entities.WarehouseManagement;

namespace Teklas_Intern_ERP.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Material Management
        public DbSet<MaterialCard> MaterialCards { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<MaterialMovement> MaterialMovements { get; set; }

        // User Management
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserTableColumnPreference> UserTableColumnPreferences { get; set; }

        // Production Management
        public DbSet<BOMHeader> BOMHeaders { get; set; }
        public DbSet<BOMItem> BOMItems { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<WorkOrderOperation> WorkOrderOperations { get; set; }
        public DbSet<ProductionConfirmation> ProductionConfirmations { get; set; }
        public DbSet<MaterialConsumption> MaterialConsumptions { get; set; }

        // Warehouse Management
        public DbSet<Warehouse> Warehouses { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<StockEntry> StockEntries { get; set; }

        // Purchasing Management
        public DbSet<SupplierType> SupplierTypes { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }

        // Sales Management
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerOrder> CustomerOrders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure each module
            ConfigureMaterialManagement(modelBuilder);
            ConfigureUserManagement(modelBuilder);
            ConfigureProductionManagement(modelBuilder);
            ConfigureWarehouseManagement(modelBuilder);
            ConfigurePurchasingManagement(modelBuilder);
            ConfigureSalesManagement(modelBuilder);

            // Configure global query filters
            ConfigureGlobalQueryFilters(modelBuilder);
        }

        private void ConfigureMaterialManagement(ModelBuilder modelBuilder)
        {
            // MaterialCard Configuration
            modelBuilder.Entity<MaterialCard>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CardCode).HasMaxLength(50).IsRequired();
                entity.Property(e => e.CardName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.CardType).HasMaxLength(50);
                entity.Property(e => e.Unit).HasMaxLength(10);
                entity.Property(e => e.PurchasePrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SalesPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CurrentStock).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MinimumStockLevel).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaximumStockLevel).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ReorderLevel).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Weight).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Volume).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Length).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Width).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Height).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Barcode).HasMaxLength(100);
                entity.Property(e => e.Brand).HasMaxLength(100);
                entity.Property(e => e.Model).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(1000);
                entity.Property(e => e.StorageLocation).HasMaxLength(100);
                entity.Property(e => e.MaterialCategoryId).IsRequired();

                // Foreign key to MaterialCategory
                entity.HasOne(mc => mc.MaterialCategory)
                      .WithMany(cat => cat.MaterialCards)
                      .HasForeignKey(mc => mc.MaterialCategoryId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint
                entity.HasIndex(e => e.CardCode).IsUnique();
            });

            // MaterialCategory Configuration
            modelBuilder.Entity<MaterialCategory>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.CategoryCode).HasMaxLength(50).IsRequired();
                entity.Property(e => e.CategoryName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);

                // Unique constraint
                entity.HasIndex(e => e.CategoryCode).IsUnique();
            });

            // MaterialMovement Configuration
            modelBuilder.Entity<MaterialMovement>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.MovementType).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);

                // Foreign key to MaterialCard
                entity.HasOne(mm => mm.MaterialCard)
                      .WithMany(mc => mc.MaterialMovements)
                      .HasForeignKey(mm => mm.MaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureUserManagement(ModelBuilder modelBuilder)
        {
            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Username).HasMaxLength(50).IsRequired();
                entity.Property(e => e.Email).HasMaxLength(100).IsRequired();
                entity.Property(e => e.PasswordHash).HasMaxLength(255).IsRequired();
                entity.Property(e => e.PasswordSalt).HasMaxLength(255).IsRequired();
                entity.Property(e => e.FirstName).HasMaxLength(100);
                entity.Property(e => e.LastName).HasMaxLength(100);
                entity.Property(e => e.PhoneNumber).HasMaxLength(20);
                entity.Property(e => e.EmailConfirmationToken).HasMaxLength(255);
                entity.Property(e => e.PasswordResetToken).HasMaxLength(255);
                entity.Property(e => e.RefreshToken).HasMaxLength(255);

                // Unique constraints
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();
            });

            // Role Configuration
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(50).IsRequired();
                entity.Property(e => e.DisplayName).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);

                // Unique constraint
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // UserRole Configuration (Many-to-Many)
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasOne(ur => ur.User)
                      .WithMany(u => u.UserRoles)
                      .HasForeignKey(ur => ur.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(ur => ur.Role)
                      .WithMany(r => r.UserRoles)
                      .HasForeignKey(ur => ur.RoleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<UserTableColumnPreference>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.TableKey).HasMaxLength(100).IsRequired();
                entity.Property(e => e.ColumnsJson).IsRequired();
                entity.Property(e => e.CreatedAt).IsRequired();
                entity.Property(e => e.UpdatedAt).IsRequired();
                entity.HasIndex(e => new { e.UserId, e.TableKey }).IsUnique();
            });
        }

        private void ConfigureProductionManagement(ModelBuilder modelBuilder)
        {
            // BOMHeader
            modelBuilder.Entity<BOMHeader>(entity =>
            {
                entity.HasKey(e => e.BOMHeaderId);
                entity.Property(e => e.Version).HasMaxLength(20);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.HasOne(e => e.ParentMaterialCard)
                      .WithMany()
                      .HasForeignKey(e => e.ParentMaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // BOMItem
            modelBuilder.Entity<BOMItem>(entity =>
            {
                entity.HasKey(e => e.BOMItemId);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ScrapRate).HasColumnType("decimal(18,2)");
                entity.HasOne(e => e.BOMHeader)
                      .WithMany(bh => bh.BOMItems)
                      .HasForeignKey(e => e.BOMHeaderId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.ComponentMaterialCard)
                      .WithMany()
                      .HasForeignKey(e => e.ComponentMaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // WorkOrder
            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.HasKey(e => e.WorkOrderId);
                entity.Property(e => e.Status).HasMaxLength(30);
                entity.HasOne(e => e.BOMHeader)
                      .WithMany()
                      .HasForeignKey(e => e.BOMHeaderId)
                      .OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(e => e.MaterialCard)
                      .WithMany()
                      .HasForeignKey(e => e.MaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // WorkOrderOperation
            modelBuilder.Entity<WorkOrderOperation>(entity =>
            {
                entity.HasKey(e => e.OperationId);
                entity.Property(e => e.OperationName).HasMaxLength(100);
                entity.Property(e => e.Resource).HasMaxLength(100);
                entity.HasOne(e => e.WorkOrder)
                      .WithMany(w => w.Operations)
                      .HasForeignKey(e => e.WorkOrderId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // ProductionConfirmation
            modelBuilder.Entity<ProductionConfirmation>(entity =>
            {
                entity.HasKey(e => e.ConfirmationId);
                entity.Property(e => e.PerformedBy).HasMaxLength(100);
                entity.HasOne(e => e.WorkOrder)
                      .WithMany()
                      .HasForeignKey(e => e.WorkOrderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // MaterialConsumption
            modelBuilder.Entity<MaterialConsumption>(entity =>
            {
                entity.HasKey(e => e.ConsumptionId);
                entity.Property(e => e.QuantityUsed).HasColumnType("decimal(18,2)");
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.BatchNumber).HasMaxLength(100);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.HasOne(e => e.ProductionConfirmation)
                      .WithMany(pc => pc.Consumptions)
                      .HasForeignKey(e => e.ConfirmationId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(e => e.MaterialCard)
                      .WithMany()
                      .HasForeignKey(e => e.MaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }

        private void ConfigureWarehouseManagement(ModelBuilder modelBuilder)
        {
            // Warehouse Configuration
            modelBuilder.Entity<Warehouse>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WarehouseCode).HasMaxLength(20).IsRequired();
                entity.Property(e => e.WarehouseName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.WarehouseType).HasMaxLength(50);
                entity.Property(e => e.Address).HasMaxLength(500);
                entity.Property(e => e.City).HasMaxLength(100);
                entity.Property(e => e.Country).HasMaxLength(100);
                entity.Property(e => e.PostalCode).HasMaxLength(20);
                entity.Property(e => e.Phone).HasMaxLength(20);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.ManagerName).HasMaxLength(100);
                entity.Property(e => e.ManagerPhone).HasMaxLength(20);
                entity.Property(e => e.ManagerEmail).HasMaxLength(100);
                entity.Property(e => e.Capacity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Temperature).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Humidity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).HasMaxLength(1000);

                // Unique constraint
                entity.HasIndex(e => e.WarehouseCode).IsUnique();
            });

            // Location Configuration
            modelBuilder.Entity<Location>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.LocationCode).HasMaxLength(20).IsRequired();
                entity.Property(e => e.LocationName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.LocationType).HasMaxLength(50);
                entity.Property(e => e.Aisle).HasMaxLength(10);
                entity.Property(e => e.Rack).HasMaxLength(10);
                entity.Property(e => e.Level).HasMaxLength(10);
                entity.Property(e => e.Position).HasMaxLength(10);
                entity.Property(e => e.Capacity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.OccupiedCapacity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Length).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Width).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Height).HasColumnType("decimal(18,2)");
                entity.Property(e => e.WeightCapacity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Temperature).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Humidity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Description).HasMaxLength(1000);

                // Foreign key to Warehouse
                entity.HasOne(l => l.Warehouse)
                      .WithMany(w => w.Locations)
                      .HasForeignKey(l => l.WarehouseId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint
                entity.HasIndex(e => e.LocationCode).IsUnique();
            });

            // StockEntry Configuration
            modelBuilder.Entity<StockEntry>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EntryNumber).HasMaxLength(20).IsRequired();
                entity.Property(e => e.EntryDate).IsRequired();
                entity.Property(e => e.EntryType).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ReferenceNumber).HasMaxLength(50);
                entity.Property(e => e.ReferenceType).HasMaxLength(50);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalValue).HasColumnType("decimal(18,2)");
                entity.Property(e => e.BatchNumber).HasMaxLength(50);
                entity.Property(e => e.SerialNumber).HasMaxLength(50);
                entity.Property(e => e.QualityStatus).HasMaxLength(50);
                entity.Property(e => e.Notes).HasMaxLength(1000);
                entity.Property(e => e.EntryReason).HasMaxLength(200);
                entity.Property(e => e.ResponsiblePerson).HasMaxLength(100);

                // Foreign key to Warehouse
                entity.HasOne(se => se.Warehouse)
                      .WithMany(w => w.StockEntries)
                      .HasForeignKey(se => se.WarehouseId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Foreign key to Location
                entity.HasOne(se => se.Location)
                      .WithMany(l => l.StockEntries)
                      .HasForeignKey(se => se.LocationId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Foreign key to MaterialCard
                entity.HasOne(se => se.Material)
                      .WithMany()
                      .HasForeignKey(se => se.MaterialId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint
                entity.HasIndex(e => e.EntryNumber).IsUnique();
            });
        }

        private void ConfigurePurchasingManagement(ModelBuilder modelBuilder)
        {
            // SupplierType Configuration
            modelBuilder.Entity<SupplierType>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.IsActive).IsRequired();

                // Unique constraint
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // Supplier Configuration
            modelBuilder.Entity<Supplier>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.TaxNumber).HasMaxLength(50);
                entity.Property(e => e.ContactPerson).HasMaxLength(50);
                entity.Property(e => e.SupplierTypeId).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();

                // Foreign key to SupplierType
                entity.HasOne(s => s.SupplierType)
                      .WithMany()
                      .HasForeignKey(s => s.SupplierTypeId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // PurchaseOrder Configuration
            modelBuilder.Entity<PurchaseOrder>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.ExpectedDeliveryDate);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.SupplierId).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();

                // Foreign key to Supplier
                entity.HasOne(po => po.Supplier)
                      .WithMany()
                      .HasForeignKey(po => po.SupplierId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint
                entity.HasIndex(e => e.OrderNumber).IsUnique();
            });
        }

        private void ConfigureSalesManagement(ModelBuilder modelBuilder)
        {
            // Customer Configuration
            modelBuilder.Entity<Customer>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).HasMaxLength(100).IsRequired();
                entity.Property(e => e.Address).HasMaxLength(200);
                entity.Property(e => e.Phone).HasMaxLength(50);
                entity.Property(e => e.Email).HasMaxLength(100);
                entity.Property(e => e.TaxNumber).HasMaxLength(50);
                entity.Property(e => e.ContactPerson).HasMaxLength(50);
                entity.Property(e => e.IsActive).IsRequired();

                // Unique constraint
                entity.HasIndex(e => e.Name).IsUnique();
            });

            // CustomerOrder Configuration
            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.OrderNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.OrderDate).IsRequired();
                entity.Property(e => e.ExpectedDeliveryDate);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)").IsRequired();
                entity.Property(e => e.Status).HasMaxLength(20).IsRequired();
                entity.Property(e => e.CustomerId).IsRequired();
                entity.Property(e => e.IsActive).IsRequired();

                // Foreign key to Customer
                entity.HasOne(co => co.Customer)
                      .WithMany()
                      .HasForeignKey(co => co.CustomerId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint
                entity.HasIndex(e => e.OrderNumber).IsUnique();
            });
        }

        private void ConfigureGlobalQueryFilters(ModelBuilder modelBuilder)
        {
            // Material Management soft delete filters
            modelBuilder.Entity<MaterialCard>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MaterialCategory>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MaterialMovement>().HasQueryFilter(x => !x.IsDeleted);

            // User Management soft delete filters
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Role>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<UserRole>().HasQueryFilter(x => !x.IsDeleted);

            // Production Management soft delete filters
            modelBuilder.Entity<BOMHeader>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<BOMItem>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<WorkOrder>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<WorkOrderOperation>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ProductionConfirmation>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MaterialConsumption>().HasQueryFilter(x => !x.IsDeleted);

            // Warehouse Management soft delete filters
            modelBuilder.Entity<Warehouse>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Location>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<StockEntry>().HasQueryFilter(x => !x.IsDeleted);

            // Purchasing Management soft delete filters
            modelBuilder.Entity<SupplierType>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Supplier>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PurchaseOrder>().HasQueryFilter(x => !x.IsDeleted);

            // Sales Management soft delete filters
            modelBuilder.Entity<Customer>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<CustomerOrder>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
} 