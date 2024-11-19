using ManagementSystem.Interfaces;
using ManagementSystem.Models;
using ManagementSystem.Utilities;

namespace ManagementSystem.DataHandling
{
    
    internal class FileInventoryManager : IInventoryManager
    {
        private const char LINE_SEPARATOR = ';';
        private readonly string _pathFile;
        public FileInventoryManager()
        {
            _pathFile = Path.Combine(Directory.GetCurrentDirectory(),"Database", "ProductFile.txt");
        }
        private  void AddToFile(Product product)
        {
            using (var writer = new StreamWriter(_pathFile, true))
            {
                writer.WriteLine(product.ToString());
            }
        }

        private  List<Product> GetProducts()
        {
            var products = new List<Product>();
            if (File.Exists(_pathFile))
            {
                ReadFromFile(products);
            }
            return products;
        }
        private void ReadFromFile(List<Product> products)
        {
            using (var reader = new StreamReader(_pathFile))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var splitedNames = line.Split(LINE_SEPARATOR);
                    if (splitedNames.Length == 3 && TryParseProduct(splitedNames, out var product))
                    {
                        Product tmp = new Product(splitedNames[0], int.Parse(splitedNames[2]), decimal.Parse(splitedNames[1]));
                        products.Add(tmp);
                    }
                }
            }
        }
        private static bool TryParseProduct(string[] splitedNames, out Product? product)
        {
            product = null;

            try
            {
                var name = splitedNames[0];
                var price = decimal.Parse(splitedNames[1]);
                var quantity = int.Parse(splitedNames[2]);
                product = new Product(name, quantity, price);
                return true;
            }
            catch (FormatException)
            {
                LogColoring.Log($"Invalid input: '{string.Join(";", splitedNames)}'");
                return false;
            }
        }
        private void WriteProductsToFile(List<Product> products)
        {
            using (var write = new StreamWriter(_pathFile))
            {
                foreach (var product in products)
                {
                    write.WriteLine(product.ToString());
                }
            }
        }


        public void AddProduct()
        {
            Product product = GetProductDetails();
            if (product != null)
            {
                try
                {
                    AddToFile(product);
                    LogColoring.Log("Product is added successfully.", ConsoleColor.Green);
                }
                catch (Exception ex)
                {

                    LogColoring.Log($"Error : {ex.Message}", ConsoleColor.Red);
                }
            }
        }
        private Product? GetProductDetails()
        {
            LogColoring.LogInline("Enter the product name: ");
            var name = Console.ReadLine();

            LogColoring.LogInline("Enter the product price: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price) || price <= 0)
            {
                LogColoring.Log("Invalid price. Please enter a valid decimal number greater than zero.", ConsoleColor.Red);
                return null;
            }

           LogColoring.LogInline("Enter the product quantity: ");
            if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity < 0)
            {
                LogColoring.Log("Invalid quantity. Please enter a valid non-negative integer.", ConsoleColor.Red);
                return null;
            }

            return new Product(name, quantity, price);
        }
        public void ListProducts(string edited)
        {
            var Products = GetProducts();
            try
            {
                LogColoring.Log("Inventory\n", ConsoleColor.Cyan);
                LogColoring.LogFormatted("{0,-25} {1,-20} {2,-15}", "Name", "Price", "Quantity");
                foreach (var product in Products)
                {
                    if (edited.Equals(product.Name) && !String.IsNullOrEmpty(edited))
                    {
                        LogColoring.LogFormatted("{0,-25} {1,-20} {2,-15}", ConsoleColor.DarkGreen,product.Name, product.Price, product.Quantity);
                    }
                    else
                    {
                        LogColoring.LogFormatted("{0,-25} {1,-20} {2,-15}", product.Name, product.Price, product.Quantity);
                    }
                }
                LogColoring.Log(String.Empty);
            }

            catch (Exception e)
            {
                LogColoring.Log($"Error occurred: '{e.Message}'", ConsoleColor.Red);
            }
        }

        public void EditProduct()
        {
            LogColoring.Log("Enter the name of the product... ");
            var name = Console.ReadLine();
            var products = GetProducts();
            var index = products.FindIndex(x => x.Name.Equals(name));

            if (index == -1)
            {
                LogColoring.Log($"The name '{name}' is not found !!", ConsoleColor.Red);
            }
            else
            {
                EditProductHelper(products, index);
                WriteProductsToFile(products);
                ListProducts(name);
            }
        }
        private void EditProductHelper(List<Product> products, int index)
        {
            bool finishedLoop = true;

            while (finishedLoop)
            {
                DisplayEditOptions(products, index);
                var picked = Console.ReadLine();

                finishedLoop = ProcessEditChoice(picked, products, index);

                if (finishedLoop)
                {
                    LogColoring.Log("Press Enter to continue...");
                    Console.ReadLine();
                }
            }
        }

        private bool ProcessEditChoice(string picked, List<Product> products, int index)
        {
            switch (picked)
            {
                case "1":
                    LogColoring.LogInline("Enter the new name: ");
                    var newName = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(newName))
                    {
                        products[index].Name = newName;
                    }
                    break;

                case "2":
                    LogColoring.LogInline("Enter the new price: ");
                    if (decimal.TryParse(Console.ReadLine(), out var newPrice) && newPrice > 0)
                    {
                        products[index].Price = newPrice;
                    }
                    break;

                case "3":
                    LogColoring.LogInline("Enter the new quantity: ");
                    if (int.TryParse(Console.ReadLine(), out var newQuantity) && newQuantity >= 0)
                    {
                        products[index].Quantity = newQuantity;
                    }
                    break;

                case "4":
                    return false;


                default:
                    LogColoring.Log("Invalid choice.", ConsoleColor.Red);
                    Console.ReadLine();
                    break;
            }

            return true;
        }


        private void DisplayEditOptions(List<Product> products, int index)
        {
            Console.Clear();
            LogColoring.Log($"You are now editing {products[index].Name}.", ConsoleColor.Cyan);
            LogColoring.Log("1. Edit the name");
            LogColoring.Log("2. Edit the price");
            LogColoring.Log("3. Edit the quantity");
            LogColoring.Log("4. Exit");
            LogColoring.LogInline("Pick an option: ");
        }

        public void DeleteProduct()
        {
            LogColoring.Log($"Enter the name of the product... ");
            string name = Console.ReadLine();
            List<Product> products = GetProducts();
            int index = products.FindIndex(x => x.Name.Equals(name));
            if (index != -1)
            {
                products.RemoveAt(index);
                WriteProductsToFile(products);
                LogColoring.Log($"The product '{name}' Deleted succesfully !!\n", ConsoleColor.Green);
            }
            else
            {
                LogColoring.Log($"The name '{name}' is not found !!", ConsoleColor.Red);
            }
        }

        public void SearchProduct()
        {
            LogColoring.Log($"Enter the name of the product... ");
            string name = Console.ReadLine();
            var products = GetProducts();
            int index = products.FindIndex(x => x.Name.Equals(name));
            if (index != -1)
            {
                LogColoring.Log("Founded :)", ConsoleColor.Green);
                LogColoring.Log($"Name: {products[index].Name} Price: {products[index].Price} Quantity: {products[index].Quantity}", ConsoleColor.DarkGreen);
            }
            else LogColoring.Log("Not Founded !!", ConsoleColor.Red);
        }
    }
}
