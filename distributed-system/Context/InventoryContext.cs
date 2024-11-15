using distributed_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace distributed_system.Context;

public class InventoryContext : DbContext
{
    public DbSet<Location> Locations { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<ProductLocation> LocationProducts { get; set; }

    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Product>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Product>()
            .Property(e => e.CostUnit)
            .HasDefaultValue(value: 0.0);

        modelBuilder.Entity<Product>()
            .Property(e => e.SalePrice)
            .HasDefaultValue(value: 0.0);

        modelBuilder.Entity<ProductLocation>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<ProductLocation>()
            .HasOne(e => e.Location)
            .WithMany()
            .HasForeignKey(e => e.LocationId);

        modelBuilder.Entity<ProductLocation>()
            .HasOne(e => e.Product)
            .WithMany()
            .HasForeignKey(e => e.ProductId);

        modelBuilder.Entity<Location>().HasData(new Location
        {
            Id = 1,
            Name = "Default Matriz",
            MaxCapacity = 100,
            CurrentCapacity = 100,
            IsMatriz = true,
            Username = "admin",
            Password = "admin"
        });
    }
}