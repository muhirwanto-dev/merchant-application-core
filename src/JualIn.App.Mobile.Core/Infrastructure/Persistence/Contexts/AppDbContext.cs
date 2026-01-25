using JualIn.Domain.Account.Entities;
using JualIn.Domain.Account.ValueObjects;
using JualIn.Domain.Catalogs.Entities;
using JualIn.Domain.Common.Entities;
using JualIn.Domain.Common.ValueObjects;
using JualIn.Domain.Inventories.Entities;
using JualIn.Domain.Inventories.ValueObjects;
using JualIn.Domain.Sales.Entities;
using JualIn.Domain.Sales.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace JualIn.App.Mobile.Core.Persistence.Contexts
{
    public class AppDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<Inventory> Inventories { get; set; }

        public DbSet<StockMovement> StockMovements { get; set; }

        public DbSet<Order> Orders { get; set; }

        public DbSet<OrderItem> OrderItems { get; set; }

        public DbSet<OrderTransaction> OrderTransactions { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<ProductComponent> ProductComponents { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                .Property(x => x.Role)
                .HasConversion(
                    toDb => toDb.Value,
                    fromDb => UserRole.FromValue(fromDb)
                );

            modelBuilder.Entity<BusinessInfo>()
                .Property(x => x.Category)
                .HasConversion<string?>(
                    toDb => toDb == null ? null : toDb.Value,
                    fromDb => fromDb == null ? null : BusinessCategory.FromValue(fromDb)
                );

            modelBuilder.Entity<RegistrationInfo>()
                .Property(x => x.BusinessCategory)
                .HasConversion<string?>(
                    toDb => toDb == null ? null : toDb.Value,
                    fromDb => fromDb == null ? null : BusinessCategory.FromValue(fromDb)
                );

            modelBuilder.Entity<Inventory>()
                .Property(x => x.InventoryId)
                .HasComputedColumnSql("('INV' || printf('%010d', Id))");
            modelBuilder.Entity<Inventory>()
                .HasIndex(x => x.InventoryId)
                .IsUnique();
            modelBuilder.Entity<Inventory>()
                .HasIndex(x => x.Name);
            modelBuilder.Entity<Inventory>()
                .Property(x => x.Stock)
                .HasConversion(
                    toDb => toDb.Value,
                    fromDb => new Stock<double>(fromDb)
                );
            modelBuilder.Entity<Inventory>()
                .Property(x => x.StockUnit)
                .HasConversion(
                    toDb => toDb.Value,
                    fromDb => StockUnit.FromValue(fromDb)
                );

            modelBuilder.Entity<StockMovement>()
                .HasIndex(x => x.OrderId);
            modelBuilder.Entity<StockMovement>()
                .HasIndex(x => x.InventoryId);
            modelBuilder.Entity<StockMovement>()
                .Property(x => x.Type)
                .HasConversion(
                    toDb => toDb.Value,
                    fromDb => StockMovementType.FromValue(fromDb)
                );
            modelBuilder.Entity<StockMovement>()
                .Property(x => x.Reason)
                .HasConversion(
                    toDb => toDb.Value,
                    fromDb => StockChangeReason.FromValue(fromDb)
                );

            //modelBuilder.Entity<Notification>()
            //    .HasIndex(x => x.Title);
            //modelBuilder.Entity<Notification>()
            //    .HasIndex(x => x.Message);

            modelBuilder.Entity<Order>()
                .HasIndex(x => x.OrderId)
                .IsUnique();
            modelBuilder.Entity<Order>()
                .Property(x => x.Status)
                .HasConversion(
                    toDb => toDb.Value,
                    fromDb => OrderStatus.FromValue(fromDb)
                );

            modelBuilder.Entity<OrderItem>()
                .HasIndex(x => x.OrderId);

            modelBuilder.Entity<OrderTransaction>()
                .HasIndex(x => x.TransactionId)
                .IsUnique();
            modelBuilder.Entity<OrderTransaction>()
                .HasIndex(x => x.OrderId);
            modelBuilder.Entity<OrderTransaction>()
                .Property(x => x.TransactionType)
                .HasConversion(
                    toDb => toDb.Value,
                    fromDb => TransactionType.FromValue(fromDb)
                );

            modelBuilder.Entity<Product>()
                .HasIndex(x => x.Name);
            modelBuilder.Entity<Product>()
                .Property(x => x.Stock)
                .HasConversion(
                    toDb => toDb.Value,
                    fromDb => new Stock<int>(fromDb)
                );
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            UpdateEntityTimestamp();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            UpdateEntityTimestamp();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void UpdateEntityTimestamp()
        {
            var now = DateTime.UtcNow;
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is AuditableEntity 
                    && (e.State == EntityState.Added || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                var auditable = (entityEntry.Entity as AuditableEntity)!;
                
                auditable.UpdatedAt = now;

                if (entityEntry.State == EntityState.Added)
                {
                    auditable.CreatedAt = now;
                }
            }
        }
    }
}
