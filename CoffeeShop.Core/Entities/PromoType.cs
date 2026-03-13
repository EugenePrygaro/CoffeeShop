namespace CoffeeShop.Core.Entities;

public enum PromoType
{
    TimeBased = 1,     // Діє на все замовлення до певної дати
    CategoryBased = 2  // Діє тільки на конкретну категорію (наприклад, "Кава")
}

public class PromoAction
{
    public int Id { get; set; }
    public string PromoCode { get; set; } // Наприклад: "BLACKFRIDAY", "COFFEE20"
    public PromoType Type { get; set; }
    public decimal DiscountPercentage { get; set; } // Відсоток знижки, наприклад 15.0
    
    // Для часових акцій
    public DateTime? ExpiryDate { get; set; } 
    
    // Для категорійних акцій (зв'язок із таблицею Category)
    public int? TargetCategoryId { get; set; } 
    public bool IsActive { get; set; } = true;
}