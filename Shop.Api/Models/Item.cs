namespace Shop.Api.Models;

public class Item
{
    public int Id { set; get; }

    public required string Name { set; get; }

    public required decimal Price { set; get; }

    public string? Description { set; get; }

    public required Category Category { set; get; }
}
