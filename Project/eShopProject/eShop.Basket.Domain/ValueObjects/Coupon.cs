namespace eShop.Basket.Domain.ValueObjects;

public class Coupon
{
    public string Code { get; set; }
    public decimal DiscountPercentage { get; set; }

    public Coupon(string code, decimal discountPercentage)
    {
        Code = code;
        DiscountPercentage = discountPercentage;
    }

    public decimal ApplyDiscount(decimal total) => total - (total * (DiscountPercentage / 100));
}
