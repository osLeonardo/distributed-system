namespace distributed_system.Entities;

public class ProductLocation
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public Location Location { get; set; }
    public int ProductId { get; set; }
    public Product Product { get; set; }
    public int Amount { get; set; }
}