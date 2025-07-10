using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.Entities.UserManagement;
using Teklas_Intern_ERP.Entities.ProductionManagement;
using Teklas_Intern_ERP.Entities;

namespace Teklas_Intern_ERP.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Material Management DbSets
        public DbSet<MaterialCard> MaterialCards { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<MaterialMovement> MaterialMovements { get; set; }

        // User Management DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        // Production Management DbSets
        public DbSet<BillOfMaterial> BillOfMaterials { get; set; }
        public DbSet<BillOfMaterialItem> BillOfMaterialItems { get; set; }
        public DbSet<WorkOrder> WorkOrders { get; set; }
        public DbSet<ProductionConfirmation> ProductionConfirmations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Material Management Configurations
            ConfigureMaterialManagement(modelBuilder);
            
            // User Management Configurations
            ConfigureUserManagement(modelBuilder);

            // Production Management Configurations
            ConfigureProductionManagement(modelBuilder);

            // Global query filters for soft delete
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
                entity.Property(e => e.MinimumStockLevel).HasColumnType("decimal(18,2)");
                entity.Property(e => e.MaximumStockLevel).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ReorderLevel).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Weight).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Volume).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Length).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Width).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Height).HasColumnType("decimal(18,2)");
                
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
        }

        private void ConfigureProductionManagement(ModelBuilder modelBuilder)
        {
            // BillOfMaterial Configuration
            modelBuilder.Entity<BillOfMaterial>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.BOMCode).HasMaxLength(50).IsRequired();
                entity.Property(e => e.BOMName).HasMaxLength(200).IsRequired();
                entity.Property(e => e.Version).HasMaxLength(20);
                entity.Property(e => e.BaseQuantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Unit).HasMaxLength(10);
                entity.Property(e => e.BOMType).HasMaxLength(50);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.RouteCode).HasMaxLength(100);
                entity.Property(e => e.StandardTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.SetupTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ApprovalStatus).HasMaxLength(20);

                // Foreign key to ProductMaterialCard
                entity.HasOne(bom => bom.ProductMaterialCard)
                      .WithMany()
                      .HasForeignKey(bom => bom.ProductMaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Unique constraint
                entity.HasIndex(e => e.BOMCode).IsUnique();
            });

            // BillOfMaterialItem Configuration
            modelBuilder.Entity<BillOfMaterialItem>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Quantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Unit).HasMaxLength(10);
                entity.Property(e => e.ScrapFactor).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ComponentType).HasMaxLength(50);
                entity.Property(e => e.IssueMethod).HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CostAllocation).HasColumnType("decimal(18,2)");

                // Foreign key to BillOfMaterial
                entity.HasOne(item => item.BillOfMaterial)
                      .WithMany(bom => bom.BOMItems)
                      .HasForeignKey(item => item.BillOfMaterialId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Foreign key to MaterialCard
                entity.HasOne(item => item.MaterialCard)
                      .WithMany()
                      .HasForeignKey(item => item.MaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Foreign key to SupplierMaterialCard (optional)
                entity.HasOne(item => item.SupplierMaterialCard)
                      .WithMany()
                      .HasForeignKey(item => item.SupplierMaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            // WorkOrder Configuration
            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.WorkOrderNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.PlannedQuantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.CompletedQuantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ScrapQuantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Unit).HasMaxLength(10);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CustomerOrderReference).HasMaxLength(100);
                entity.Property(e => e.WorkCenter).HasMaxLength(100);
                entity.Property(e => e.Shift).HasMaxLength(50);
                entity.Property(e => e.PlannedSetupTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.PlannedRunTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ActualSetupTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ActualRunTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.WorkOrderType).HasMaxLength(50);
                entity.Property(e => e.SourceType).HasMaxLength(50);
                entity.Property(e => e.CompletionPercentage).HasColumnType("decimal(18,2)");
                entity.Property(e => e.QualityStatus).HasMaxLength(20);

                // Foreign key to BillOfMaterial
                entity.HasOne(wo => wo.BillOfMaterial)
                      .WithMany(bom => bom.WorkOrders)
                      .HasForeignKey(wo => wo.BillOfMaterialId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Foreign key to ProductMaterialCard
                entity.HasOne(wo => wo.ProductMaterialCard)
                      .WithMany()
                      .HasForeignKey(wo => wo.ProductMaterialCardId)
                      .OnDelete(DeleteBehavior.Restrict);

                // Foreign key to SupervisorUser (optional)
                entity.HasOne(wo => wo.SupervisorUser)
                      .WithMany()
                      .HasForeignKey(wo => wo.SupervisorUserId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Foreign key to ReleasedByUser (optional)
                entity.HasOne(wo => wo.ReleasedByUser)
                      .WithMany()
                      .HasForeignKey(wo => wo.ReleasedByUserId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Unique constraint
                entity.HasIndex(e => e.WorkOrderNumber).IsUnique();
            });

            // ProductionConfirmation Configuration
            modelBuilder.Entity<ProductionConfirmation>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ConfirmationNumber).HasMaxLength(50).IsRequired();
                entity.Property(e => e.ConfirmedQuantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ScrapQuantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.ReworkQuantity).HasColumnType("decimal(18,2)");
                entity.Property(e => e.Unit).HasMaxLength(10);
                entity.Property(e => e.WorkCenter).HasMaxLength(100);
                entity.Property(e => e.Status).HasMaxLength(20);
                entity.Property(e => e.ConfirmationType).HasMaxLength(20);
                entity.Property(e => e.SetupTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.RunTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DownTime).HasColumnType("decimal(18,2)");
                entity.Property(e => e.DownTimeReason).HasMaxLength(200);
                entity.Property(e => e.Shift).HasMaxLength(50);
                entity.Property(e => e.Notes).HasMaxLength(500);
                entity.Property(e => e.QualityStatus).HasMaxLength(20);
                entity.Property(e => e.QualityNotes).HasMaxLength(500);
                entity.Property(e => e.BatchNumber).HasMaxLength(50);
                entity.Property(e => e.SerialNumberFrom).HasMaxLength(50);
                entity.Property(e => e.SerialNumberTo).HasMaxLength(50);
                entity.Property(e => e.CostCenter).HasMaxLength(50);

                // Foreign key to WorkOrder
                entity.HasOne(pc => pc.WorkOrder)
                      .WithMany(wo => wo.ProductionConfirmations)
                      .HasForeignKey(pc => pc.WorkOrderId)
                      .OnDelete(DeleteBehavior.Cascade);

                // Foreign key to OperatorUser (optional)
                entity.HasOne(pc => pc.OperatorUser)
                      .WithMany()
                      .HasForeignKey(pc => pc.OperatorUserId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Foreign key to ConfirmedByUser (optional)
                entity.HasOne(pc => pc.ConfirmedByUser)
                      .WithMany()
                      .HasForeignKey(pc => pc.ConfirmedByUserId)
                      .OnDelete(DeleteBehavior.NoAction);

                // Unique constraint
                entity.HasIndex(e => e.ConfirmationNumber).IsUnique();
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
            modelBuilder.Entity<BillOfMaterial>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<BillOfMaterialItem>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<WorkOrder>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<ProductionConfirmation>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
} 