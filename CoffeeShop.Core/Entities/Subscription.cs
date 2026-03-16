namespace CoffeeShop.Core.Entities;

public enum SubscriptionType
{
    None = 0,
    BasicCoffeePass = 1,
    PremiumRoasterClub = 2
}

public class Subscription
{
    public int Id { get; set; }
    public SubscriptionType Type { get; set; }
    public decimal Price { get; set; }
    public int DurationInDays { get; set; }
    public decimal DiscountPercentage { get; set; }

    public List<Customer> Customers { get; set; } = new();
}