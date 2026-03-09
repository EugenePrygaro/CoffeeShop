namespace CoffeeShop.Core.Entities;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public int? SubscriptionId { get; set; } 
    public Subscription? Subscription { get; set; } 
}