using distributed_system.Entities;
using Microsoft.EntityFrameworkCore;

namespace distributed_system.Context;

public class InventoryContext : DbContext
{
    public DbSet<Location> Locations { get; set; }

    public InventoryContext(DbContextOptions<InventoryContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Location>().HasKey(l => l.Id);
    }

}