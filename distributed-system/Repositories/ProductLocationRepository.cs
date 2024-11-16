using distributed_system.Context;
using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Repositories;

public class ProductLocationRepository : IProductLocationRepository
{
    private readonly InventoryContext _context;
    private readonly ILogger<ProductLocationRepository> _logger;

    public ProductLocationRepository(
        InventoryContext context,
        ILogger<ProductLocationRepository> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public ActionResult AddProductLocation(ProductLocation productLocation)
    {
        try
        {
            _context.LocationProducts.Add(productLocation);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestResult();
        }

        return new OkResult();
    }

    public ActionResult UpdateProductLocation(ProductLocation productLocation)
    {
        try
        {
            _context.LocationProducts.Update(productLocation);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestResult();
        }

        return new OkResult();
    }

    public ProductLocation GetProductLocationById(int id)
    {
        ProductLocation productLocation = new();

        try
        {
            productLocation = _context.LocationProducts.FirstOrDefault(l =>
                l.Id == id
            );
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar localização: " + ex.Message);
        }

        return productLocation;
    }

    public List<ProductLocation> GetAllProductLocations()
    {
        List<ProductLocation> productLocations = new();

        try
        {
            productLocations = _context.LocationProducts.ToList();
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar localização: " + ex.Message);
        }

        return productLocations;
    }

    public ActionResult DeleteProductLocation(int id)
    {
        try
        {
            var productLocation = _context.LocationProducts.FirstOrDefault(l =>
                l.Id == id
            );

            if (productLocation == null)
            {
                return new NotFoundResult();
            }

            _context.LocationProducts.Remove(productLocation);
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