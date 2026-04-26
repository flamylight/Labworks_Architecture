using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }
    
    public DbSet<Service> Services { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderService> OrderServices { get; set; }
    public DbSet<PortfolioItem> PortfolioItems { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);

        optionsBuilder.UseNpgsql(
            Environment.GetEnvironmentVariable("DB_CONNECTION")
        );
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OrderService>()
            .HasKey(os => new { os.OrderId, os.ServiceId });

        modelBuilder.Entity<OrderService>()
            .HasOne(os => os.Order)
            .WithMany(o => o.OrderServices)
            .HasForeignKey(os => os.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<OrderService>()
            .HasOne(os => os.Service)
            .WithMany()
            .HasForeignKey(os => os.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<PortfolioItem>()
            .HasOne(p => p.Order)
            .WithOne()
            .HasForeignKey<PortfolioItem>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Service>()
            .Property(s => s.Price)
            .HasPrecision(18, 2);
    }
}