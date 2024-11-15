using distributed_system.Context;
using distributed_system.Entities;
using distributed_system.Repositories.Intefaces;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly InventoryContext _context;
    private readonly ILogger<ProductRepository> _logger;

    public ProductRepository(
        InventoryContext context,
        ILogger<ProductRepository> logger
    )
    {
        _context = context;
        _logger = logger;
    }

    public ActionResult AddProduct(Product product)
    {
        try
        {
            _context.Products.Add(product);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestResult();
        }

        return new OkResult();
    }

    public ActionResult UpdateProduct(Product product)
    {
        try
        {
            _context.Products.Update(product);
            _context.SaveChanges();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            return new BadRequestResult();
        }

        return new OkResult();
    }

    public Product GetProductByName(string name)
    {
        Product product = new();

        try
        {
            product = _context.Products.FirstOrDefault(l =>
                l.Name == name
            );
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao buscar localização: " + ex.Message);
        }

        return product;
    }

    public ActionResult DeleteProduct(int id, string name)
    {
        try
        {
            var product = _context.Products.FirstOrDefault(l =>
                l.Id == id &&
                l.Name == name
            );

            if (product == null)
            {
                return new NotFoundResult();
            }

            _context.Products.Remove(product);
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
