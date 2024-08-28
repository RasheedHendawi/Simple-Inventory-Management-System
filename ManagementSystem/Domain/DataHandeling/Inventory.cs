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
            using (StreamWriter W= new StreamWriter(pathFile, true))   //using keyword = the finally{W.Dispose();} which ensures closing and true here is to append
            {
                W.WriteLine(p.ToString());
            }
        }
    }
}
