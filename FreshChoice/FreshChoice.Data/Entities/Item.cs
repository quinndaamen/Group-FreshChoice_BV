namespace FreshChoice.Data.Entities;

public class Item : Entity
{
    public string Name { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public ItemCategory Category { get; set; }
}