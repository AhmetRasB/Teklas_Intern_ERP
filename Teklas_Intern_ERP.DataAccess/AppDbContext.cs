using Microsoft.EntityFrameworkCore;
using Teklas_Intern_ERP.Entities.MaterialManagement;
using Teklas_Intern_ERP.Entities;

namespace Teklas_Intern_ERP.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<MaterialCard> MaterialCards { get; set; }
        public DbSet<MaterialCategory> MaterialCategories { get; set; }
        public DbSet<MaterialMovement> MaterialMovements { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MaterialCard>(entity =>
            {
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
            });

            // Global query filters for soft delete
            modelBuilder.Entity<MaterialCard>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MaterialCategory>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<MaterialMovement>().HasQueryFilter(x => !x.IsDeleted);
        }
    }
} 