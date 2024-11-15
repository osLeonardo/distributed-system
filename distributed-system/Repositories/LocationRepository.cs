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
    private readonly ILogger<LocationRepository> _logger;

    public LocationRepository(
        InventoryContext context,
        ILogger<LocationRepository> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public ActionResult AddLocation(Location location)
    {
        try
        {
            _context.Locations.Add(location);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestResult();
        }

        return new OkResult();
    }

    public ActionResult UpdateLocation(Location location)
    {
        try
        {
            _context.Locations.Update(location);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestResult();
        }

        return new OkResult();
    }

    public Location GetLocationByName(string name)
    {
        Location location = new();

        try
        {
            location = _context.Locations.FirstOrDefault(l =>
                l.Name == name
            );
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar localização: " + ex.Message);
        }

        return location;
    }

    public Location GetLocationByUsernameAndPassword(string username, string password)
    {
        try
        {
            return _context.Locations.FirstOrDefault(l => l.Username == username && l.Password == password);
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching location: " + ex.Message);
        }
    }

    public ActionResult DeleteLocation(int id, string name)
    {
        try
        {
            var location = _context.Locations.FirstOrDefault(l =>
                l.Id == id &&
                l.Name == name
            );

            if (location == null)
            {
                return new NotFoundResult();
            }

            _context.Locations.Remove(location);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestResult();
        }

        return new OkResult();
    }
}