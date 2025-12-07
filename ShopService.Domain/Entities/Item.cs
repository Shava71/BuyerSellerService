using ShopService.Domain.ValueObject;

namespace ShopService.Domain.Entities;

public class Item
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public Category Category { get; set; }
    public decimal Price { get; set; }
    public bool IsSold { get;  set; }
    
    // навигация
    public Guid? PurchaseId { get; private set; }
    public Purchase? Purchase { get; private set; }

    private Item() { } 

    public Item(Guid id, string name, Category category, decimal price)
    {
        Id = id; Name = name; Category = category; Price = price; IsSold = false;
    }

    public void Update(string name, Category category, decimal price)
    {
        Name = name; Category = category; Price = price;
    }

    public void MarkSold(Guid purchaseId)
    {
        if (IsSold)
        {
            throw new InvalidOperationException("Item already sold");
        }
        IsSold = true;
        PurchaseId = purchaseId;
    }
}