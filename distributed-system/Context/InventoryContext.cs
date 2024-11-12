using distributed_system.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace distributed_system.Context;

public class InventoryContext : DbContext
{
    public DbSet<Location> Locations { get; set; }

    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var guidToStringConverter = new ValueConverter<Guid, string>(
            v => v.ToString(),
            v => Guid.Parse(v));

        modelBuilder.Entity<Location>()
            .Property(e => e.Id)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Location>().HasData(new Location
        {
            Id = 1,
            Name = "Default Matriz",
            MaxCapacity = 100,
            CurrentCapacity = 0,
            IsMatriz = true
        });
    }
}