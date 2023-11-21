namespace ItemRegistry;

public class ItemRegistry{
    // Creating collection of items
    public List<Item> Items = new() {
        new Item(1, 30, "Soap"), 
        new Item(2, 60, "Duck", true), 
        new Item(3, 12, "Bread"), 
        new Item(4, 22.5, "Coffee"),
        new Item(5, 30, "Oranges", true), 
        new Item(6, 12, "Sugar"),
        new Item(7, 85.00, "Pork", true),
        new Item(8, 10,  "Milk")
    };


    // Fetches the price of an item given the id
    public double GetPrice(int id){
        var item = Items.First(item => item.Id == id);
        return item.Price;
    }

    // Fetches the name of an item given the id
    public string GetName(int id){
        var item = Items.First(item => item.Id == id);
        return item.Name;
    }
}

// The item information
public class Item {
    public int Id { get; init; }
    public double Price { get; init; } 
    public string Name { get; init; } = string.Empty;
    public bool HasWeightPrice = false;  //Indicates if the item has price/kg 

    public Item(int id, double price, string name, bool hasWeightPrice = false) {
        Price = price;
        Id = id;
        Name = name;
        HasWeightPrice = hasWeightPrice;
    }
}

//Creating global single instance of the item registry
public sealed class Singleton {
    public static Singleton Instance { get; } = new Singleton();
    private readonly ItemRegistry _ItemRegistry = new();
    public ItemRegistry ItemRegistry { get { return _ItemRegistry; } }
}