using distributed_system.Entities;
using Microsoft.AspNetCore.Mvc;

namespace distributed_system.Repositories.Intefaces;

public interface IProductRepository
{
    public ActionResult AddProduct(Product product);
    public ActionResult UpdateProduct(Product product);
    public Product GetProductByName(string name);
    Product GetProductById(int id);
    public List<Product> GetAllProducts();
    public ActionResult DeleteProduct(int id, string name);
}