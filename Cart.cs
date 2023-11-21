using ItemRegistry;
using Discounts;

namespace Cart;

public class CheckoutItem {
    //Implementing a special class in order to make the price computation easier
    public int Id { get; init; }
    public CheckoutItem (int id) {
        Id = id;
    }
}
public class CheckoutItemWithWeight : CheckoutItem {
    public double Weight { get; init; }
    public CheckoutItemWithWeight(int id, double weight) : base(id) {
        Id = id;
        Weight = weight;
    }
}
public class CheckoutUtility{

    public static double Sum(List<CheckoutItem> items){
        var registry = Singleton.Instance.ItemRegistry;
        double result = 0;

        foreach(var item in items){
            if(item is CheckoutItemWithWeight ciww) {
                result += ciww.Weight*registry.GetPrice(ciww.Id);
            }
            else {
                result += registry.GetPrice(item.Id);
            }
        }
        return result;
    }
}

public class Checkout {

    // String used to print discounts
    private string AppliedDiscounts = string.Empty;

    // List that stores purchases
    private readonly List<CheckoutItem> Items = new();

    // Attempted to implement discounts as reusable as possible
    private readonly List<Discount> Discounts = new() {
        new TwoCountDiscount() {
            Id = 4, // Coffee
            DiscountAmount = 5 // The difference between two packs of coffee with and without discount
        },
        new ThreeForTwoDiscount() {
            Id = 1 // Toothpaste
        },
        new PriceThresholdDiscount {
            Id = 5, // Apple
            Threshold = 150,
            NewPrice = 16.95 // New price of product with Id, in this case apple
        }
    };

    // Adds a single item
    public void AddItem(int id) {

        var registry = Singleton.Instance.ItemRegistry.Items;
        var item = registry.First(item => item.Id == id && !item.HasWeightPrice);
        Items.Add(new CheckoutItem(item.Id));
    }

    // Adds a single weighted item
    public void AddItem(int id, double weight){

        var registry = Singleton.Instance.ItemRegistry.Items;
        var item = registry.First(item => item.Id == id && item.HasWeightPrice);
        Items.Add(new CheckoutItemWithWeight(item.Id, weight));
    }

    // Calculates and returns the total price after discounts
    public double Sum() {

        var afterDiscounts = CheckoutUtility.Sum(Items);
        var registry = Singleton.Instance.ItemRegistry;

        foreach (var discount in Discounts) {
            var savedPrice = discount.CalculateDiscount(Items);
            if (savedPrice > 0){
                AppliedDiscounts += registry.GetName(discount.Id) + " discount: " + savedPrice.ToString() + "\n";
            }
            afterDiscounts -= discount.CalculateDiscount(Items);
        }
        return afterDiscounts;
    }

    // Prints the total price after discountss
    public void Print() {
        Console.WriteLine("Total price: " + Sum());
        Console.WriteLine(AppliedDiscounts);
    }
}