using Cart;
using ItemRegistry;

namespace Discounts;

//Base class for discounts
public abstract class Discount {
    public int Id { get; init; }
    public abstract double CalculateDiscount(List<CheckoutItem> items);
}

//General two count discount later to be applied on coffee
public class TwoCountDiscount : Discount
{
    public double DiscountAmount { get; init; }
    public override double CalculateDiscount(List<CheckoutItem> items) {
        
        var amountOfMatchedItems = items.Count(item => item.Id == Id);
    
        if (amountOfMatchedItems % 2 == 0){
            return amountOfMatchedItems/2 * DiscountAmount;
        }
        else {
            return (amountOfMatchedItems - 1)/2 * DiscountAmount;
        }
    }
}

//General Three for two discount later to be applied on toothpaste
public class ThreeForTwoDiscount : Discount
{
    public override double CalculateDiscount(List<CheckoutItem> items)
    {
        var amountOfMatchedItems = items.Count(item => item.Id == Id);
        var registry = Singleton.Instance.ItemRegistry;
        double discount = 0;

        while(amountOfMatchedItems >= 3){
            discount += registry.GetPrice(Id);
            amountOfMatchedItems -= 3;
        }
        return discount;
    }
}

//General price treshold discount later to be applied on apples
public class PriceThresholdDiscount : Discount
{   
    public double Threshold { get; init; }
    public double NewPrice { get; init; }
    public override double CalculateDiscount(List<CheckoutItem> items)
    {
        var registry = Singleton.Instance.ItemRegistry;
        var discountedItems = items.Where(item => item.Id == Id);
        var rest = items.Where(item => item.Id != Id).ToList();
        var totalDiscount = 0.0;


        if (CheckoutUtility.Sum(rest) >= Threshold) {
            foreach (var discountedItem in discountedItems)
            {
                //If discounted item has weight then the weight has to be accounted for in discount
                if (discountedItem is CheckoutItemWithWeight ciww) {
                    totalDiscount += (registry.GetPrice(ciww.Id) - NewPrice) * ciww.Weight;
                } else {
                    totalDiscount += registry.GetPrice(discountedItem.Id) - NewPrice;
                }
            }
        }
        return totalDiscount;
    }
}