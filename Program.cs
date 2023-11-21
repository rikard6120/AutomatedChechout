using Cart;

var checkout = new Checkout();

// Adding items
checkout.AddItem(1);
checkout.AddItem(1);
checkout.AddItem(1);


checkout.AddItem(1);
checkout.AddItem(1);
checkout.AddItem(2, 1);
checkout.AddItem(3);
checkout.AddItem(4);
checkout.AddItem(4);
checkout.AddItem(4);
checkout.AddItem(5, 1.8);
checkout.AddItem(6);
checkout.AddItem(7, 1);
checkout.AddItem(8);

// Printing sum and discounts
checkout.Print();
