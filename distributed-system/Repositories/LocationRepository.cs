using System.Threading.Tasks;
using distributed_system.Context;
using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Repositories;

public class LocationRepository : ILocationRepository
{
    private readonly InventoryContext _context;

    public LocationRepository(InventoryContext context)
    {
        _context = context;
    }

    public ActionResult AddLocation(Location location)
    {
        _context.Locations.Add(location);
        _context.SaveChangesAsync();
        return new OkResult();
    }

    public Location GetLocationByName(string name)
    {
        return _context.Locations.FirstOrDefault(l => l.Name == name);
    }
}