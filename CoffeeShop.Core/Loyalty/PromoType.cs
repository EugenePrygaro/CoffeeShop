namespace CoffeeShop.Core.Loyalty;

public enum PromoType
{
    TimeBased = 1,     
    CategoryBased = 2  
}

public class PromoAction
{
    public int Id { get; set; }
    public string PromoCode { get; set; } = string.Empty;
    public PromoType Type { get; set; }
    public decimal DiscountPercentage { get; set; } 
    
    public DateTime? ExpiryDate { get; set; } 
    public int? TargetCategoryId { get; set; } 
    public bool IsActive { get; set; } = true;
}