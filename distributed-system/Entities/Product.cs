namespace distributed_system.Entities;

public class Product
{
    public int Id { get; set; }
    public string Brand { get; set; }
    public string Name { get; set; }
    public decimal CostUnit { get; set; }
    public decimal SalePrice { get; set; }
}