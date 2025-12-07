namespace ShopService.Domain.Entities;

public class Purchase
{
    public Guid Id { get; private set; }
    public decimal Price => Items.Sum(x => x.Price);
    public DateTime PurchasedAt { get; private set; }
    public List<Item> Items { get; private set; }
    
    private Purchase() { }

    public Purchase(Guid id, DateTime purchasedAt, IEnumerable<Item> items)
    {
        Id = id; PurchasedAt = purchasedAt; Items = items.ToList();
    }
}