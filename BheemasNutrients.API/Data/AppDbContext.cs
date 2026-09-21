using BheemasNutrients.API.Models;
using Microsoft.EntityFrameworkCore;

namespace BheemasNutrients.API.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items)
            .WithOne()
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.Price)
            .HasPrecision(18, 2);
    }
}
