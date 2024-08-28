using ManagementSystem.Domain.ProdcutManagment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem.Domain
{
    internal class Inventory
    {
        private static string pathFile = "C:/Users/rrash/Desktop/Simple-Inventory-Management-System/ManagementSystem/Domain/DataHandeling/ProductRepository/ProductFile.txt";
        public static void AddProduct(Product p)
        {
            using (StreamWriter W= new StreamWriter(pathFile, true))   //using keyword = the finally{W.Dispose();} which ensures closing... and true here is to append
            {
                W.WriteLine(p.ToString());
            }
        }
        public static List<Product> ViewProduct()
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
                                Console.WriteLine($"Invalid input: {R.ReadLine()}");
                            }
                        }
                    }
                }
               
            }
            return products;
        }
    }
}
