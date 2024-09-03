using System;
using System.Drawing;
using ManagementSystem;
using ManagementSystem.Domain;
using ManagementSystem.Domain.DataHandeling;
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
                Console.Write("Pick an option 1 => 6 :");
                Console.ResetColor();
                var picked=Console.ReadLine();

                switch (picked)
                {
                    case "1":
                        Inventory.AddProduct();
                        break;
                    case "2":
                        Inventory.ListProducts();
                        break;
                    case "3":
                        Inventory.EditProduct();
                        break;
                    case "4":
                        Inventory.DeleteProduct();
                        break;
                    case "5":
                        Inventory.SearchProduct();
                        break;
                    case "6":
                        return;

                    default:
                        FormatColoring.MyLogMessage("Invalid choice.", ConsoleColor.Red);
                        break;
                }
                Console.WriteLine("Press Enter to continue...");
                Console.ReadLine();// important so the consol dont close and wait for messages
            }
        }
        
    }
}
