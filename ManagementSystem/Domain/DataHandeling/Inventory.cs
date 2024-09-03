using ManagementSystem.Domain.DataHandeling;
using ManagementSystem.Domain.ProdcutManagment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem.Domain
{
    internal class Inventory
    {
        private static string pathFile = "C:/Users/rrash/Desktop/Simple-Inventory-Management-System/ManagementSystem/Domain/DataHandeling/ProductRepository/ProductFile.txt";
        private static void AddProductToTheFile(Product p)
        {
            using (StreamWriter W= new StreamWriter(pathFile, true))   //using keyword = the finally{W.Dispose();} which ensures closing... and true here is to append
            {
                W.WriteLine(p.ToString());
            }
        }
        public static void AddProduct()
        {
            string name;
            decimal price;
            int quantity;
            Console.Write("Enter the product name: ");
            name = Console.ReadLine();
            Console.Write("Enter the product price: ");
            price = Decimal.Parse(Console.ReadLine());
            Console.Write("Enter the product quantity: ");
            quantity = Int32.Parse(Console.ReadLine());
            Product product = new Product(name, quantity, price);
            try
            {
                AddProductToTheFile(product);
                FormatColoring.MyLogMessage("Product is added successfully.", ConsoleColor.Green);
            }
            catch (Exception ex)
            {

                FormatColoring.MyLogMessage($"Error : {ex.Message}", ConsoleColor.Red);
            }

        }
        public static void ListProducts(string edited,bool editEnable)
        {
            List<Product> listo = ViewProduct();
            try
            {
                FormatColoring.MyLogMessage("Inventory", ConsoleColor.Green);
                Console.WriteLine("{0,-25} {1,-20} {2,-15} ", "Name", "Price", "Quntity\n");
                foreach (Product p in listo)
                {
                    if (editEnable)
                    {
                        if (edited.Equals(p.Name))
                        {
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            Console.WriteLine("{0,-25} {1,-20} {2,-15}", p.Name, p.Price, p.Quantity);
                            Console.ResetColor();
                        }
                        else
                            Console.WriteLine("{0,-25} {1,-20} {2,-15}", p.Name, p.Price, p.Quantity);
                    }
                }
                Console.WriteLine();
            }
            catch (Exception e)
            {
                FormatColoring.MyLogMessage($"Error occurred'{e}'", ConsoleColor.Red);
            }
        }
        private static List<Product> ViewProduct()
        {
            List<Product> products = new List<Product>();
            if (File.Exists(pathFile))
            {
                using (StreamReader R = new StreamReader(pathFile))
                {
                    string line;
                    while ((line=R.ReadLine() )!= null)
                    {
                        var names = line.Split(';');
                        if (names.Length == 3)
                        {
                            try 
                            {
                                Product tmp = new Product(names[0], Int32.Parse(names[2]), decimal.Parse(names[1]));
                                products.Add(tmp);
                            }
                            catch (FormatException)
                            {
                                Console.WriteLine($"Invalid input: '{R.ReadLine()}'");
                            }
                        }
                    }
                }
               
            }
            return products;
        }
        public static void EditProduct()
        {
            Console.WriteLine($"Enter the name of the product... ");
            string name=Console.ReadLine();
            List<Product> products = ViewProduct();
            int index = products.FindIndex(x => x.Name.Equals(name));
            if (index != -1)
            {
                bool FinishedLoop = true;
                while (FinishedLoop)
                {
                    Console.Clear();
                    FormatColoring.MyLogMessage($"You are now Editing {products[index].Name}.", ConsoleColor.Green);
                    Console.WriteLine("1. Edit the name");
                    Console.WriteLine("2. Edit the price");
                    Console.WriteLine("3. Edit the quantity");
                    Console.WriteLine("4. Exit");
                    Console.Write("Pick an option :");
                    var picked = Console.ReadLine();
                    switch (picked)
                    {
                        case "1":
                            Console.WriteLine($"Enter the new name: ");
                            string s = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(s))
                            {
                                products[index].Name = s;
                            }
                            break;
                        case "2":
                            Console.WriteLine($"Enter the new price: ");
                            string p = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(p))
                            {
                                if (decimal.Parse(p) > 0)
                                {
                                    products[index].Price = decimal.Parse(p);
                                }
                            }
                            break;
                        case "3":
                            Console.WriteLine($"Enter the new quantity: ");
                            string q = Console.ReadLine();
                            if (!string.IsNullOrWhiteSpace(q))
                            {
                                //if(int.Parse(q)>0)
                                if(int.TryParse(q,out int newQ) && newQ>=0)
                                {
                                    products[index].Quantity = newQ;
                                }
                            }
                            break;
                        case "4":
                            FinishedLoop = false;
                            break;
                        default:
                            FormatColoring.MyLogMessage("Invalid choice.", ConsoleColor.Red);
                            Console.ReadLine();
                            continue;
                    }
                    if (FinishedLoop)
                    {
                        Console.WriteLine("Press Enter to continue...");
                        Console.ReadLine();// important so the consol dont close and wait for messages
                    }
                }
                WriteProductOnFile(products);
                ListProducts(name,true);
            }
            else
            {
                FormatColoring.MyLogMessage($"The name '{name}' is not found !!",ConsoleColor.Red);
            }
        }
        private static void WriteProductOnFile(List<Product> products)
        {
            using (StreamWriter write = new StreamWriter(pathFile))
            {
                foreach (var p in products)
                {
                    write.WriteLine(p.ToString());
                }
            }
        }
        public static void DeleteProduct()
        {
            Console.WriteLine($"Enter the name of the product... ");
            string name = Console.ReadLine();
            List<Product> products = ViewProduct();
            int index = products.FindIndex(x => x.Name.Equals(name));
            if (index != -1)
            {
                products.RemoveAt(index);
                WriteProductOnFile(products);
                FormatColoring.MyLogMessage($"The product '{name}' Deleted succesfully !!\n", ConsoleColor.Green);
            }
            else
            {
                FormatColoring.MyLogMessage($"The name '{name}' is not found !!", ConsoleColor.Red);
            }
        }

        public static void SearchProduct()
        {
            Console.WriteLine($"Enter the name of the product... ");
            string name = Console.ReadLine();
            var p= ViewProduct();
            int i = p.FindIndex(x => x.Name.Equals(name));
            if (i != -1)
            {
                FormatColoring.MyLogMessage("Founded :)",ConsoleColor.Green);
                FormatColoring.MyLogMessage($"Name: {p[i].Name} Price: {p[i].Price} Quantity: {p[i].Quantity}", ConsoleColor.DarkGreen);
            }
            else FormatColoring.MyLogMessage("Not Founded !!", ConsoleColor.Red);
        }
    }
}
