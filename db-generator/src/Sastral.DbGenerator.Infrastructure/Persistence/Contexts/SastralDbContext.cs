using Microsoft.EntityFrameworkCore;
using Sastral.DbGenerator.Domain.Entities.Users;
using Sastral.DbGenerator.Domain.Entities.Products;
using Sastral.DbGenerator.Domain.Entities.Orders;
using Sastral.DbGenerator.Domain.Entities.Carts;
using Sastral.DbGenerator.Domain.Entities.Payments;
using Sastral.DbGenerator.Domain.Entities.Shipments;
using Sastral.DbGenerator.Domain.Entities.Inventory;
using Sastral.DbGenerator.Domain.Entities.Audits;
using Sastral.DbGenerator.Application.Abstractions;

namespace Sastral.DbGenerator.Infrastructure.Persistence.Contexts;

public class SastralDbContext : DbContext, IUnitOfWork
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductVariant> ProductVariants => Set<ProductVariant>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Shipment> Shipments => Set<Shipment>();
    public DbSet<InventoryMovement> InventoryMovements => Set<InventoryMovement>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public SastralDbContext(DbContextOptions<SastralDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Apply all configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SastralDbContext).Assembly);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
