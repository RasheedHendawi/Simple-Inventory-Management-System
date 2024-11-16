using ManagementSystem.DataHandling;
using ManagementSystem.Interfaces;
using ManagementSystem.Utilities;
using Microsoft.Extensions.Configuration;

namespace ManagementSystem
{
    internal class Program
    {
        static void Main()
        {
            DisplayHeader();
            while (true)
            {
                DisplayWelcomeMessage();
                var picked = Console.ReadLine()?.Trim();
                HandleUserChoice(picked);
            }
        }
        private static void DisplayHeader()
        {
            string header = @"
                                             (      (                 )      *           
                             (  (            )\ )   )\ )     (     ( /(    (  `          
                             )\))(   ' (    (()/(  (()/(     )\    )\())   )\))(    (    
                            ((_)()\ )  )\    /(_))  /(_))  (((_)  ((_)\   ((_)()\   )\   
                            _(())\_)()((_)  (_))   (_))    )\___    ((_)  (_()((_) ((_)  
                            \ \((_)/ /| __| | |    | |    ((/ __|  / _ \  |  \/  | | __| 
                             \ \/\/ / | _|  | |__  | |__   | (__  | (_) | | |\/| | | _|  
                              \_/\_/  |___| |____| |____|   \___|  \___/  |_|  |_| |___| 
                                                             
                                                                                         
                                ********************************************************
                                *                                                      *
                                *       Welcome to the Inventory Management System     *
                                *                                                      *
                                ********************************************************
    ";

            LogColoring.Log(header, ConsoleColor.DarkRed);
        }

        private static void DisplayWelcomeMessage()
        {
            string welcomingPage = @"
  Please select an option from the menu below:

  [1] > Add a product
  [2] > View all products
  [3] > Edit a product
  [4] > Delete a product
  [5] > Search for a product
  [6] > Exit";
            LogColoring.Log(welcomingPage, ConsoleColor.Cyan);
            LogColoring.LogInline("\n  Enter your choice(1 - 6):",ConsoleColor.Cyan);
        }

        private static void HandleUserChoice(string? picked)
        {
            var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();
            var connecter = configuration.GetConnectionString("InventoryDB");
            if (string.IsNullOrEmpty(connecter))
            {
                LogColoring.LogFormatted("Error: Connection string is missing or invalid." +
                    " Please check your appsettings.json file.", ConsoleColor.Red);
                return;
            }
            IInventoryManager inventory = new SqlInventoryManager(connecter);
            //IInventoryManager inventory = new FileInventoryManager();
            switch (picked)
            {
                case "1":
                    inventory.AddProduct();
                    break;
                case "2":
                    inventory.ListProducts(String.Empty);
                    break;
                case "3":
                    inventory.EditProduct();
                    break;
                case "4":
                    inventory.DeleteProduct();
                    break;
                case "5":
                    inventory.SearchProduct();
                    break;
                case "6":
                    Environment.Exit(0);
                    break;

                default:
                    LogColoring.Log("Invalid choice.", ConsoleColor.Red);
                    break;
            }
            LogColoring.Log("Press Enter to continue...");
            Console.ReadLine();
        }
    }
}