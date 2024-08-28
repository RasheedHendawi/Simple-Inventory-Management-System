using System;
using System.Drawing;
using ManagementSystem;
using ManagementSystem.Domain;
using ManagementSystem.Domain.ProdcutManagment;
namespace ManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                Console.ForegroundColor=ConsoleColor.Cyan;
                Console.WriteLine("Welcome to the Inventory Managment System");
                Console.WriteLine("1. Add a product");
                Console.WriteLine("2. View all products");
                Console.WriteLine("3. Edit a product");
                Console.WriteLine("4. Delete a product");
                Console.WriteLine("5. Search for a product");
                Console.WriteLine("6. Exit", ConsoleColor.Yellow);
                Console.Write("Pick an option 1 -> 6 :");
                Console.ResetColor();
                var picked=Console.ReadLine();

                switch (picked)
                {
                    case "1":
                        AddProduct();
                        break;
                    case "6":
                        return;

                    default:
                        MyLogMessage("Invalid choice.", ConsoleColor.Red);
                        break;
                }
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();// important so the consol dont close and wait for messages
            }
        }
        static void MyLogMessage(string message, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        private static void AddProduct()
        {
            string name;
            decimal price;
            int quantity;
            Console.Write("Enter the product name: ");
            name= Console.ReadLine();
            Console.Write("Enter the product price: ");
            price=Decimal.Parse(Console.ReadLine());
            Console.Write("Enter the product quantity: ");
            quantity=Int32.Parse(Console.ReadLine());
            Product product = new Product(name,quantity,price);
            try
            {
                Inventory.AddProduct(product);
                MyLogMessage("Product is added successfully.", ConsoleColor.Green);
            }
            catch (Exception ex)
            {

                MyLogMessage($"Error : {ex.Message}", ConsoleColor.Red);
            }

        }
    }
}
