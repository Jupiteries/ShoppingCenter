using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ShoppingCentre
{
    public class Shopping // This will be the main class. All the shopping is executed here.
    {
        public static void Welcome(Client client) // Welcomes the client and saves the basic information about them. Checks the blacklist to see if the client is allowed to visit. Calls either the method Shop(), or Return().
        {
            Console.WriteLine("[Cashier] - Welcome to our shopping centre. Before you start shopping, I will ask for your credentials.");
        wrongFormat:
            Console.WriteLine("Please enter your ID: ");
            if (!long.TryParse(Console.ReadLine(), out long enteredId))
            {
                Console.WriteLine("Write an integer!");
                goto wrongFormat;
            }
            if (enteredId.ToString().Length != 9)
            {
                Console.WriteLine("Your id must be 9 digits long!");
                goto wrongFormat;
            }
            client.Id = enteredId;
            string overallBlackLists = File.ReadAllText("overallblacklist.json");
            var overallBlackListsList = JsonConvert.DeserializeObject<List<Blacklists>>(overallBlackLists);

            try
            {
                if (overallBlackListsList is not null)
                {
                    foreach (var blacklist in overallBlackListsList)
                    {
                        if (blacklist.Id == client.Id)
                        {
                            Console.WriteLine("You are blacklisted from all the shops.");
                            Environment.Exit(0);
                        }

                    }
                }

            }
            catch { }

        wrongNameFormat:
            Console.WriteLine("Please enter your first name:");
            client.FirstName = Console.ReadLine();
            if (string.IsNullOrEmpty(client.FirstName) || string.IsNullOrWhiteSpace(client.FirstName))
            {
                Console.WriteLine("Enter your actual name.");
                goto wrongNameFormat;
            }
            wrongLastNameFormat:
            Console.WriteLine("And your last name:");
            client.LastName = Console.ReadLine();
            if (string.IsNullOrEmpty(client.LastName) || string.IsNullOrWhiteSpace(client.LastName))
            {
                Console.WriteLine("Enter your actual name.");
                goto wrongLastNameFormat;
            }

            Random rand = new Random();
            Wallet clientsWallet = new Wallet(client.Id, rand.Next(20, 99), client.TimesInPrison); // The users money will be randomly generated between 20-99 dollars.
            
            try
            {
                Console.WriteLine("All the information has been collected. Do you want to:\nStart shopping (keyword 'shop')\nYou've already been to our shops and you want to return your products (keyword 'return')");
                string userAnswer = Console.ReadLine();

                if (userAnswer == "shop")
                {

                    Console.WriteLine("Now, Please choose the shop you want to visit first.");
                    Shop(client, clientsWallet);
                }
                else
                {
                    Return(client);
                }
            }
            catch
            {
                Console.WriteLine("You can't return any products, you haven't even shopped here before.");
            }
        }

        static void Return(Client client) // If the client wants to return their products
        {
            Random rand = new Random();
            int timeSinceShopping = rand.Next(1, 58); // This will determine how many hours it has been since the client last shopped here.
            string clientsWalletString = File.ReadAllText($"{client.Id}wallet.json");
            Wallet? clientsWallet = System.Text.Json.JsonSerializer.Deserialize<Wallet>(clientsWalletString);

            if (File.Exists($"{client.Id}.json"))
            {
                string checkFile = File.ReadAllText($"{client.Id}.json");
                Check? check = System.Text.Json.JsonSerializer.Deserialize<Check>(checkFile);

                if (timeSinceShopping <= 24)
                {
                    Console.WriteLine($"{client.FirstName}, you will be returning these products:");

                    foreach (var product in check.ChosenProducts)
                    {
                        Console.WriteLine(product.ProductName);
                    }
                    Console.WriteLine($"{check.FinalPricePlusTaxes} dollars will be added to your wallet.");
                    clientsWallet.MoneyInWallet += check.FinalPricePlusTaxes;

                    foreach (var product in check.ChosenProducts)
                    {
                        // Now we are checking every shop and returning the bought products.
                        string euroFruitsAndVegString = File.ReadAllText($"eurofruitsandveg.json");
                        var euroFruitsAndVegList = JsonConvert.DeserializeObject<List<Product>>(euroFruitsAndVegString);

                        foreach (var anotherProduct in euroFruitsAndVegList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        string euroDessertsAndPastryString = File.ReadAllText($"eurodessertsandpastry.json");
                        var euroDessertsAndPastryList = JsonConvert.DeserializeObject<List<Product>>(euroDessertsAndPastryString);
                        foreach (var anotherProduct in euroDessertsAndPastryList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        string euroSnacksString = File.ReadAllText($"eurosnacks.json");
                        var euroSnacksList = JsonConvert.DeserializeObject<List<Product>>(euroSnacksString);
                        foreach (var anotherProduct in euroSnacksList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        string nikoraFruitsAndVegString = File.ReadAllText($"nikorafruitsandveg.json");
                        var nikoraFruitsAndVegList = JsonConvert.DeserializeObject<List<Product>>(nikoraFruitsAndVegString);
                        foreach (var anotherProduct in nikoraFruitsAndVegList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        string nikoraCannedFoodsString = File.ReadAllText($"nikoracannedfoods.json");
                        var nikoraCannedFoodsList = JsonConvert.DeserializeObject<List<Product>>(nikoraCannedFoodsString);
                        foreach (var anotherProduct in nikoraCannedFoodsList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        string nikoraHygieneProductsString = File.ReadAllText($"nikorahygieneproducts.json");
                        var nikoraHygieneProductsList = JsonConvert.DeserializeObject<List<Product>>(nikoraHygieneProductsString);
                        foreach (var anotherProduct in nikoraHygieneProductsList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        string goodwillFruitsAndVegString = File.ReadAllText($"goodwillfruitsandveg.json");
                        var goodwillFruitsAndVegList = JsonConvert.DeserializeObject<List<Product>>(euroFruitsAndVegString);
                        foreach (var anotherProduct in goodwillFruitsAndVegList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        string goodwillSnacksString = File.ReadAllText($"goodwillsnacks.json");
                        var goodwillSnacksList = JsonConvert.DeserializeObject<List<Product>>(goodwillSnacksString);
                        foreach (var anotherProduct in goodwillSnacksList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        string goodwillDrinksString = File.ReadAllText($"goodwilldrinks.json");
                        var goodwillDrinksList = JsonConvert.DeserializeObject<List<Product>>(goodwillDrinksString);
                        foreach (var anotherProduct in goodwillDrinksList)
                        {
                            if (anotherProduct.ProductName == product.ProductName)
                            {
                                anotherProduct.ProductAmount += 5;
                            }
                        }

                        // Deserializing/saving.
                        using (FileStream fs = new FileStream("eurofruitsandveg.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, euroFruitsAndVegList);
                        }

                        using (FileStream fs = new FileStream("eurodessertsandpastry.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, euroDessertsAndPastryList);
                        }

                        using (FileStream fs = new FileStream("eurosnacks.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, euroSnacksList);
                        }

                        // NIKORA
                        using (FileStream fs = new FileStream("nikorafruitsandveg.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, nikoraFruitsAndVegList);
                        }

                        using (FileStream fs = new FileStream("nikoracannedfoods.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, nikoraCannedFoodsList);
                        }

                        using (FileStream fs = new FileStream("nikorahygieneproducts.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, nikoraHygieneProductsList);
                        }

                        // GOODWIll
                        using (FileStream fs = new FileStream("goodwillfruitsandveg.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, goodwillFruitsAndVegList);
                        }

                        using (FileStream fs = new FileStream("goodwillsnacks.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, goodwillSnacksList);
                        }

                        using (FileStream fs = new FileStream("goodwilldrinks.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, goodwillDrinksList);
                        }
                    }

                    Console.WriteLine("Returning...");
                    Console.WriteLine("Returning...\n\n");
                    Console.WriteLine($"{client.FirstName}, the products have been returned. Have a nice day!");
                }
                else if (timeSinceShopping > 24 & timeSinceShopping <= 48)
                {
                    Console.WriteLine("It's been over 24 hours since you purchased these products:\n");
                    foreach (var product in check.ChosenProducts)
                    {
                        Console.WriteLine(product.ProductName);
                    }
                    Console.WriteLine("\n\n{{The Cashier is calling the Police}}");
                    Console.WriteLine("\n\n{{The Police came and put you in a protocol.}}");
                }
                else
                {
                    Console.WriteLine("It's been over 48 hours since you purchased these products:\n");

                    foreach (var product in check.ChosenProducts)
                    {
                        Console.WriteLine(product.ProductName);
                    }

                    Console.WriteLine("\n\n{{The Cashier is calling the Police}}");
                    Console.WriteLine("{{The police came and gave you a fine of 80 dollars. Please, pay 80 dollars now.}}");
                    Console.WriteLine("Checking your wallet...");

                    // In the assignment it said that the fee was 1000 dollars, but in this project the client can't possibly have that much, so I'm making a change and only asking for 80.
                    // In the assignment it said that the fee was 1000 dollars, but in this project the client can't possibly have that much, so I'm making a change and only asking for 80.
                    if (clientsWallet.MoneyInWallet >= 80)
                    {
                        Console.WriteLine("You paid 1000 dollars and were let go. Don't let this happen again!");
                    }
                    else
                    {
                        Console.WriteLine($"{client.FirstName} {client.LastName}, since you don't have enough money, you're going to prison for 5 years. You're also in a blacklist of our shops, you will never shop here again. Goodbye.)");
                        // Here we are adding the clients ID to the blacklist.
                        Blacklists clientBlacklist = new Blacklists(client.Id);

                        string overallBlackLists = File.ReadAllText("overallblacklist.json");
                        var overallBlackListsList = JsonConvert.DeserializeObject<List<Blacklists>>(overallBlackLists);
                        overallBlackListsList.Add(clientBlacklist);
                        using (FileStream fs = new FileStream("overallblacklist.json", FileMode.OpenOrCreate))
                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                        {
                            System.Text.Json.JsonSerializer.Serialize(writer, overallBlackListsList);
                        }

                        client.TimesInPrison += 1;

                        if (clientsWallet.TimesInPrison > 0)
                        {
                            Console.WriteLine("So this isn't the first time you're going to prison. In that case, you're going for 100 years!");
                        }

                    }
                }
            }
            else
            {
                Console.WriteLine("You have never even shopped here! Go and shop first.");
                Console.WriteLine("Please choose the shop you want to visit first.");

                Shop(client, clientsWallet);
            }
            using (FileStream fs = new FileStream($"{client.Id}wallet.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, clientsWallet);
            }

        }
        public static void Shop(Client client, Wallet clientsWallet) // Shopping process
        {

            double finalPrice = 0;

            // THE COMMENTED CODE BELOW IS SERIALIZATION/CREATION OF ALL THE SHOPS AND CATEGORIES. IF SOMETHING GETS DELETED BY ACCIDENT YOU CAN REMOVE THE "/* */" AND CREATE THE CATEGORIES AGAIN.

            // EUROPRODUCT
            var europroductFruitsAndVegetables = new List<Product>();
            europroductFruitsAndVegetables.Add(new Product("Banana", 55, 0.5, 9, 0));
            europroductFruitsAndVegetables.Add(new Product("Tomato", 45, 0.4, 5, 20));
            europroductFruitsAndVegetables.Add(new Product("Potato", 90, 0.3, 7, 10));
            europroductFruitsAndVegetables.Add(new Product("Eggplant", 40, 0.6, 10, 0));
            europroductFruitsAndVegetables.Add(new Product("Carrot", 110, 0.2, 3, 30));

            var europroductDessertsAndPastry = new List<Product>();
            europroductDessertsAndPastry.Add(new Product("Chocolate cake", 5, 12, 7, 10));
            europroductDessertsAndPastry.Add(new Product("Chocolate pudding", 15, 3, 11, 0));
            europroductDessertsAndPastry.Add(new Product("Strawberry yogurt", 20, 3, 3, 30));
            europroductDessertsAndPastry.Add(new Product("Khachapuri", 9, 6.5, 8, 0));

            var europroductSnacks = new List<Product>();
            europroductSnacks.Add(new Product("Tic-tac", 20, 1, 50, 0));
            europroductSnacks.Add(new Product("M&ms", 20, 1.2, 50, 0));
            europroductSnacks.Add(new Product("Chocolate bar", 15, 1.5, 50, 0));
            europroductSnacks.Add(new Product("Kinder", 30, 0.8, 50, 0));
            europroductSnacks.Add(new Product("Sour patch candy", 30, 0.9, 50, 0));
            europroductSnacks.Add(new Product("Ice cream box", 10, 4, 50, 0));

            // NIKORA
            var nikoraFruitsAndVegetables = new List<Product>();
            nikoraFruitsAndVegetables.Add(new Product("Apple", 110, 0.5, 7, 10));
            nikoraFruitsAndVegetables.Add(new Product("Pear", 80, 0.6, 2, 30));
            nikoraFruitsAndVegetables.Add(new Product("Cucumber", 35, 0.3, 1, 50));
            nikoraFruitsAndVegetables.Add(new Product("Watermelon", 10, 0.7, 9, 0));
            nikoraFruitsAndVegetables.Add(new Product("Lemon", 40, 0.5, 3, 30));

            var nikoraCannedFoods = new List<Product>();
            nikoraCannedFoods.Add(new Product("Peanuts", 40, 3, 13, 0));
            nikoraCannedFoods.Add(new Product("Sardines", 30, 3, 1, 50));
            nikoraCannedFoods.Add(new Product("Beans", 30, 2.6, 3, 30));
            nikoraCannedFoods.Add(new Product("Canned tomatoes", 30, 2.4, 6, 10));

            var nikoraHygieneProducts = new List<Product>();  // These don't expire
            nikoraHygieneProducts.Add(new Product("Toothbrush", 30, 2, 100, 0));
            nikoraHygieneProducts.Add(new Product("Scrub", 30, 1.5, 100, 0));
            nikoraHygieneProducts.Add(new Product("Toothpaste", 30, 2.5, 100, 0));
            nikoraHygieneProducts.Add(new Product("Razor (pack of 5)", 40, 5, 100, 0));
            nikoraHygieneProducts.Add(new Product("Mint gum (pack of 30)", 40, 5, 100, 0));

            // GOODWILL
            var goodwillFruitsAndVegetables = new List<Product>();
            goodwillFruitsAndVegetables.Add(new Product("Strawberries", 2000, 0.03, 7, 10));
            goodwillFruitsAndVegetables.Add(new Product("Mandarin", 40, 0.3, 5, 20));
            goodwillFruitsAndVegetables.Add(new Product("Orange", 40, 0.3, 8, 0));
            goodwillFruitsAndVegetables.Add(new Product("Dragonfruit", 15, 1, 9, 0));
            goodwillFruitsAndVegetables.Add(new Product("Avocado", 30, 0.7, 2, 30));

            var goodwillSnacks = new List<Product>(); // These don't expire
            goodwillSnacks.Add(new Product("Oreos (12 pieces)", 20, 4, 100, 0));
            goodwillSnacks.Add(new Product("Snickers", 60, 0.8, 100, 0));
            goodwillSnacks.Add(new Product("Twix", 60, 0.8, 100, 0));
            goodwillSnacks.Add(new Product("Crackers", 30, 1.2, 100, 0));
            goodwillSnacks.Add(new Product("Sour candy", 40, 0.9, 100, 0));
            goodwillSnacks.Add(new Product("Gum", 400, 0.05, 100, 0));
            goodwillSnacks.Add(new Product("Doritos", 30, 2, 100, 0));
                                                                                           
            var goodwillDrinks = new List<Product>();
            goodwillDrinks.Add(new Product("Coca-cola", 10, 2, 100, 0));
            goodwillDrinks.Add(new Product("Fanta", 10, 2, 100, 0));
            goodwillDrinks.Add(new Product("Red bull", 20, 4.5, 100, 0));
            goodwillDrinks.Add(new Product("Mountain dew", 15, 3, 100, 0));
            goodwillDrinks.Add(new Product("Orange juice", 10, 2, 4, 20));
            goodwillDrinks.Add(new Product("Apple juice", 10, 2, 4, 20));
            goodwillDrinks.Add(new Product("Cherry juice", 10, 2, 4, 20));

            // EUROPRODUCT
            using (FileStream fs = new FileStream("eurofruitsandveg.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, europroductFruitsAndVegetables);
            }
            using (FileStream fs = new FileStream("eurodessertsandpastry.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, europroductDessertsAndPastry);
            }
            using (FileStream fs = new FileStream("eurosnacks.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, europroductSnacks);
            }

            // NIKORA
            using (FileStream fs = new FileStream("nikorafruitsandveg.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, nikoraFruitsAndVegetables);
            }
            using (FileStream fs = new FileStream("nikoracannedfoods.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, nikoraCannedFoods);
            }
            using (FileStream fs = new FileStream("nikorahygieneproducts.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, nikoraHygieneProducts);
            }

            // GOODWIll
            using (FileStream fs = new FileStream("goodwillfruitsandveg.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, goodwillFruitsAndVegetables);
            }
            using (FileStream fs = new FileStream("goodwillsnacks.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, goodwillSnacks);
            }
            using (FileStream fs = new FileStream("goodwilldrinks.json", FileMode.OpenOrCreate))
            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
            {
                System.Text.Json.JsonSerializer.Serialize(writer, goodwillDrinks);
            }

            List<Product> chosenProductsList = new List<Product>(); // List of the products the client chooses

        back:
            Console.WriteLine("\n\nEuroproduct (keyword 'euro') Rating: 4.6\nNikora (keyword 'nikora') Rating: 4.7\nGoodwill (keyword 'goodwill') Rating: 4.9");
            string usersAnswer = Console.ReadLine();
            string chosenCategoryToEnter;

            switch (usersAnswer)
            {

                case "euro":
                    Console.WriteLine("Welcome to Europroduct, please choose which category you wish to enter (Make sure to enter the keyword correctly!).\n");
                    Console.WriteLine("Fruits and Vegetables (keyword 'eurofruitsandveg')\nDesserts and Pastry (keyword 'eurodessertsandpastry')\nSnacks (keyword 'eurosnacks')\nBack (keyword 'back')");
                    chosenCategoryToEnter = Console.ReadLine();
                    break;
                case "nikora":
                    Console.WriteLine("Welcome to Nikora, please choose which category you wish to enter (Make sure to enter the keyword correctly!).\n");
                    Console.WriteLine("Fruits and Vegetables (keyword 'nikorafruitsandveg')\n Canned Foods (keyword 'nikoracannedfoods')\nHygiene products (keyword 'nikorahygieneproducts')\nBack (keyword 'back')");
                    chosenCategoryToEnter = Console.ReadLine();
                    break;

                case "goodwill":
                    Console.WriteLine("Welcome to europroduct, please choose which category you wish to enter (Make sure to enter the keyword correctly!).\n");
                    Console.WriteLine("Fruits and Vegetables (keyword 'goodwillfruitsandveg')\nSnacks (keyword 'goodwillsnacks')\nDrinks (keyword 'goodwilldrinks')\nBack (keyword 'back')");
                    chosenCategoryToEnter = Console.ReadLine();
                    break;
                default:
                    Console.WriteLine("You misspelled.");
                    goto back;
                    break;
            }

            if (chosenCategoryToEnter is not "back")
            {
                string chosenCategory = File.ReadAllText($"{chosenCategoryToEnter}.json");
                var chosenCategoryProductsList = JsonConvert.DeserializeObject<List<Product>>(chosenCategory);
                Console.WriteLine("\n\nChoose the products you want!");

            choose:
                foreach (var one in chosenCategoryProductsList)
                {
                    Console.WriteLine($"\nProduct: {one.ProductName} ('keyword: {one.ProductName.ToLower()}'), amount: {one.ProductAmount}, price: {one.ProductPrice} dollars, expires in {one.ExpiresIn} days, therefore, {one.PercentOff} percent off. (FinalPrice: {one.ProductPrice - one.ProductPrice * one.PercentOff / 100})");
                }
                Console.WriteLine("\nBack(keyword 'back')");
                Console.WriteLine("\nWhenever you want to stop shopping and go to the register, type 'stop'.");




            putReturn:
                while (true)
                {
                    Console.WriteLine("Choose the product you want to buy:");
                    string productChosen = Console.ReadLine();

                    if (productChosen == "back")
                    {
                        goto back;
                    }
                    if (productChosen == "stop")
                    {
                        Console.WriteLine("Are you sure that you want to stop shopping? ('y'/'n')");
                        char answer = Convert.ToChar(Console.ReadLine());
                        if (answer == 'y')
                        {

                            Check(client, clientsWallet, finalPrice, chosenProductsList);
                            Environment.Exit(0);
                        }
                        else
                        {
                            Console.WriteLine("Then, let's continue shopping.");
                            goto choose;
                        }

                    }
            outOfStockOption:
                    Console.WriteLine($"Now, choose how many {productChosen}(s) you want:");
                    int productAmount = Convert.ToInt32(Console.ReadLine());

                    foreach (var product in chosenCategoryProductsList)
                    {
                        if (product.ProductName == char.ToUpper(productChosen[0]) + productChosen.Substring(1))
                        {
                            if (product.ProductAmount - productAmount >= 0)
                            {
                                product.ProductAmount -= productAmount;
                            } else
                            {
                                Console.WriteLine($"We do not have that many {product.ProductName}. Choose less.");
                                goto outOfStockOption;
                            }

                            Console.WriteLine($"You've chosen {productAmount} {product.ProductName}(s). The product has been added to the cart.");
                            Console.WriteLine("\n\nIf you want to put the product back, type 'put'. If not, type 'continue'\n");

                            string clientsAnswer = Console.ReadLine();
                            if (clientsAnswer == "put")
                            {
                                foreach (var secondTime in chosenCategoryProductsList)
                                {
                                    if (secondTime.ProductName == product.ProductName)
                                    {
                                        secondTime.ProductAmount += productAmount;

                                        Console.WriteLine("\n\nThe product has been put back on the shelf. You can buy some other products you like\n");
                                        foreach (var thirdTime in chosenCategoryProductsList)
                                        {
                                            Console.WriteLine($"\nProduct: {thirdTime.ProductName} ('keyword: {thirdTime.ProductName.ToLower()}'), amount: {thirdTime.ProductAmount}, price: {thirdTime.ProductPrice} dollars, expires in {thirdTime.ExpiresIn} days, therefore, {thirdTime.PercentOff} percent off. (FinalPrice: {thirdTime.ProductPrice - thirdTime.ProductPrice * thirdTime.PercentOff / 100})");
                                        }
                                        Console.WriteLine("\nBack(keyword 'back')");
                                        Console.WriteLine("\nWhenever you want to stop shopping and go to the register, type 'stop'.");
                                        using (FileStream fs = new FileStream($"{chosenCategoryToEnter}.json", FileMode.OpenOrCreate))
                                        using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                                        {
                                            System.Text.Json.JsonSerializer.Serialize(writer, chosenCategoryProductsList);
                                        }
                                        chosenCategory = File.ReadAllText($"{chosenCategoryToEnter}.json");
                                        chosenCategoryProductsList = JsonConvert.DeserializeObject<List<Product>>(chosenCategory);

                                        goto putReturn;
                                    }
                                }
                            }
                            else
                            {
                                finalPrice += productAmount * (product.ProductPrice - product.ProductPrice * product.PercentOff / 100);
                                Console.WriteLine("\nThe product has been put in the cart and the price has been tracked.\n");

                                chosenProductsList.Add(product);

                                foreach (var one in chosenCategoryProductsList)
                                {
                                    Console.WriteLine($"\nProduct: {one.ProductName} ('keyword: {one.ProductName.ToLower()}'), amount: {one.ProductAmount}, price: {one.ProductPrice} dollars, expires in {one.ExpiresIn} days, therefore, {one.PercentOff} percent off. (FinalPrice: {one.ProductPrice - one.ProductPrice * one.PercentOff / 100})");
                                }
                                Console.WriteLine("\nBack(keyword 'back')");
                                Console.WriteLine("\nWhenever you want to stop shopping and go to the register, type 'stop'.");
                                using (FileStream fs = new FileStream($"{chosenCategoryToEnter}.json", FileMode.OpenOrCreate))
                                using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                                {
                                    System.Text.Json.JsonSerializer.Serialize(writer, chosenCategoryProductsList);
                                }
                                chosenCategory = File.ReadAllText($"{chosenCategoryToEnter}.json");
                                chosenCategoryProductsList = JsonConvert.DeserializeObject<List<Product>>(chosenCategory);
                                goto putReturn;
                            }
                            Console.WriteLine("\n\nChoose the products you want!");
                        }
                    }
                }
            }
            else
            {
                goto back;
            }


        }
        static void Check(Client client, Wallet clientsWallet, double finalPrice, List<Product> chosenProductsList) // Making a check
        {
            Random rand = new Random();
            int guid = rand.Next(1000, 9999);
            double finalPricePlusTaxes = finalPrice + (finalPrice * 0.18); // The overall price of the products plus 18 percent as tax.


            Console.WriteLine("\n\nHere are the types of products you chose: ");
            int count = 1;
            foreach (var product in chosenProductsList)
            {
                Console.WriteLine($"Product {count}: {product.ProductName}, price: {product.ProductPrice}");
                count++;
            }
            Console.WriteLine($"\nThe final price is {finalPricePlusTaxes}.");
            if (clientsWallet.MoneyInWallet >= finalPrice)
            {
                Console.WriteLine($"[Cashier] - Mr(s). {client.FirstName}, here is your check:\n\n");
                Check check = new Check(guid, finalPricePlusTaxes, "Nikoloz", "Dilsizi", client.FirstName, client.LastName, client.Id, chosenProductsList);
                string fileName = $"{client.Id}.json";
                string checkString = System.Text.Json.JsonSerializer.Serialize(check);
                File.WriteAllText(fileName, checkString);
                clientsWallet.MoneyInWallet -= finalPricePlusTaxes;

                Console.WriteLine($"CHECK ({check.GUID})\n");
                Console.WriteLine($"Final Price: {finalPricePlusTaxes} dollars.\nProducts bought: ");

                foreach (var product in chosenProductsList)
                {
                    Console.WriteLine($"{product.ProductName} price: {product.ProductPrice} (expires in {product.ExpiresIn} days)");
                }
                Console.WriteLine($"\nCashier credentials: \nName: Nikoloz\nSurname: Dilsizi\n");
                Console.WriteLine($"Clients credentials: \nName: {client.FirstName}\nSurname: {client.LastName}\nID: {client.Id}.");

                using (FileStream fs = new FileStream($"{client.Id}wallet.json", FileMode.OpenOrCreate))
                using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                {
                    System.Text.Json.JsonSerializer.Serialize(writer, clientsWallet);
                }
            }
            else
            {
                Console.WriteLine($"Mr(s). {client.LastName}, you don't have enough money. You lack {finalPricePlusTaxes - clientsWallet.MoneyInWallet} dollars. Please put some products back:" +
                    $"n");
                while (true)
                {
                    foreach (var product in chosenProductsList)
                    {
                        Console.WriteLine($"{product.ProductName} (keyword '{product.ProductName.ToLower()}'), price: {product.ProductPrice} ");
                    }
                    Console.WriteLine("Which product will you put back?");
                    string putBackProduct = Console.ReadLine();
                    Console.WriteLine("How many will you put back?");
                    double amount = Convert.ToInt32(Console.ReadLine());

                    foreach (var product in chosenProductsList)
                    {
                        if (product.ProductName == char.ToUpper(putBackProduct[0]) + putBackProduct.Substring(1))
                        {
                            clientsWallet.MoneyInWallet += product.ProductPrice * amount;
                            finalPricePlusTaxes -= product.ProductPrice * amount;

                            // deserialize every single category and change the amounts
                            string euroFruitsAndVegString = File.ReadAllText($"eurofruitsandveg.json");
                            var euroFruitsAndVegList = JsonConvert.DeserializeObject<List<Product>>(euroFruitsAndVegString);
                            foreach (var anotherProduct in euroFruitsAndVegList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }
                            string euroDessertsAndPastryString = File.ReadAllText($"eurodessertsandpastry.json");
                            var euroDessertsAndPastryList = JsonConvert.DeserializeObject<List<Product>>(euroDessertsAndPastryString);
                            foreach (var anotherProduct in euroDessertsAndPastryList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }
                            string euroSnacksString = File.ReadAllText($"eurosnacks.json");
                            var euroSnacksList = JsonConvert.DeserializeObject<List<Product>>(euroSnacksString);
                            foreach (var anotherProduct in euroSnacksList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }

                            string nikoraFruitsAndVegString = File.ReadAllText($"nikorafruitsandveg.json");
                            var nikoraFruitsAndVegList = JsonConvert.DeserializeObject<List<Product>>(nikoraFruitsAndVegString);
                            foreach (var anotherProduct in nikoraFruitsAndVegList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }
                            string nikoraCannedFoodsString = File.ReadAllText($"nikoracannedfoods.json");
                            var nikoraCannedFoodsList = JsonConvert.DeserializeObject<List<Product>>(nikoraCannedFoodsString);
                            foreach (var anotherProduct in nikoraCannedFoodsList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }
                            string nikoraHygieneProductsString = File.ReadAllText($"nikorahygieneproducts.json");
                            var nikoraHygieneProductsList = JsonConvert.DeserializeObject<List<Product>>(nikoraHygieneProductsString);
                            foreach (var anotherProduct in nikoraHygieneProductsList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }

                            string goodwillFruitsAndVegString = File.ReadAllText($"goodwillfruitsandveg.json");
                            var goodwillFruitsAndVegList = JsonConvert.DeserializeObject<List<Product>>(euroFruitsAndVegString);
                            foreach (var anotherProduct in goodwillFruitsAndVegList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }
                            string goodwillSnacksString = File.ReadAllText($"goodwillsnacks.json");
                            var goodwillSnacksList = JsonConvert.DeserializeObject<List<Product>>(goodwillSnacksString);
                            foreach (var anotherProduct in goodwillSnacksList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }
                            string goodwillDrinksString = File.ReadAllText($"goodwilldrinks.json");
                            var goodwillDrinksList = JsonConvert.DeserializeObject<List<Product>>(goodwillDrinksString);
                            foreach (var anotherProduct in goodwillDrinksList)
                            {
                                if (anotherProduct.ProductName == product.ProductName)
                                {
                                    anotherProduct.ProductAmount += Convert.ToInt32(amount);
                                }
                            }



                            using (FileStream fs = new FileStream("eurofruitsandveg.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, euroFruitsAndVegList);
                            }
                            using (FileStream fs = new FileStream("eurodessertsandpastry.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, euroDessertsAndPastryList);
                            }
                            using (FileStream fs = new FileStream("eurosnacks.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, euroSnacksList);
                            }

                            // NIKORA
                            using (FileStream fs = new FileStream("nikorafruitsandveg.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, nikoraFruitsAndVegList);
                            }
                            using (FileStream fs = new FileStream("nikoracannedfoods.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, nikoraCannedFoodsList);
                            }
                            using (FileStream fs = new FileStream("nikorahygieneproducts.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, nikoraHygieneProductsList);
                            }

                            // GOODWIll
                            using (FileStream fs = new FileStream("goodwillfruitsandveg.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, goodwillFruitsAndVegList);
                            }
                            using (FileStream fs = new FileStream("goodwillsnacks.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, goodwillSnacksList);
                            }
                            using (FileStream fs = new FileStream("goodwilldrinks.json", FileMode.OpenOrCreate))
                            using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                            {
                                System.Text.Json.JsonSerializer.Serialize(writer, goodwillDrinksList);
                            }



                        }

                    }

                    if (clientsWallet.MoneyInWallet < finalPricePlusTaxes)
                    {
                        Console.WriteLine($"Still not enough. You lack {finalPricePlusTaxes - clientsWallet.MoneyInWallet} Put back more products.");

                    }
                    else
                    {
                        Console.WriteLine("You now have enough money. Here is your check:\n\n");
                        Check check = new Check(guid, finalPricePlusTaxes, "Nikoloz", "Dilsizi", client.FirstName, client.LastName, client.Id, chosenProductsList);
                        
                        string fileName = $"{client.Id}.json";
                        string checkString = System.Text.Json.JsonSerializer.Serialize(check);
                        File.WriteAllText(fileName, checkString);
                        clientsWallet.MoneyInWallet -= finalPricePlusTaxes;
                        Console.WriteLine($"CHECK ({check.GUID})\n");
                        Console.WriteLine($"Final Price: {finalPricePlusTaxes} dollars.\nProducts bought: ");

                        foreach (var product in chosenProductsList)
                        {
                            Console.WriteLine($"{product.ProductName} price: {product.ProductPrice} (expires in {product.ExpiresIn} days)");
                        }
                        Console.WriteLine($"\nCashier credentials: \nName: Nikoloz\nSurname: Dilsizi\n");
                        Console.WriteLine($"Clients credentials: \nName: {client.FirstName}\nSurname: {client.LastName}\nID: {client.Id}.");
                        break;
                    }
                }


                using (FileStream fs = new FileStream($"{client.Id}wallet.json", FileMode.OpenOrCreate))
                using (Utf8JsonWriter writer = new Utf8JsonWriter(fs))
                {
                    System.Text.Json.JsonSerializer.Serialize(writer, clientsWallet);
                }



            }


        }
    }
}



