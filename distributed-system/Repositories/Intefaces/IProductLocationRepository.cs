using distributed_system.Entities;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Repositories.Intefaces;

public interface IProductLocationRepository
{
    ActionResult AddProductLocation(ProductLocation productLocation);
    ActionResult UpdateProductLocation(ProductLocation productLocation);
    ProductLocation GetProductLocationById(int id);
    List<ProductLocation> GetAllProductLocations();
    ActionResult DeleteProductLocation(int id);
}