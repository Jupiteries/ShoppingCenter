namespace ShoppingCentre
{
    public class Product 
    {
        public string ProductName { get; set; } // Apple, Banana, Chocolate Cake, etc.
        public int ProductAmount { get; set; } // How many in stock
        public double ProductPrice { get; set; } // In dollars
        public int ExpiresIn { get; set; } // In how many days it expires (If it expires in more than 7 days, then there is no discount.)
        public int PercentOff { get; set; } // What % off

        public Product(string productName, int productAmount, double productPrice, int expiresIn, int percentOff)
        {
            ProductName = productName;
            ProductAmount = productAmount;
            ProductPrice = productPrice;
            ExpiresIn = expiresIn;
            PercentOff = percentOff;
        }
    }
}
