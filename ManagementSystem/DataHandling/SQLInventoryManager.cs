using ManagementSystem.Interfaces;
using ManagementSystem.Models;

namespace ManagementSystem.DataHandling
{
    public class SqlInventoryManager : IInventoryManager
    {
        private readonly SQLProductRepository _repository;

        public SqlInventoryManager(string connectionString)
        {
            _repository = new SQLProductRepository(connectionString);
        }

        public void AddProduct()
        {
            var product = GetProductDetails();
            if (product != null)
            {
                _repository.AddProduct(product);
                Console.WriteLine("Product added successfully.");
            }
        }

        public void ListProducts(string edited)
        {
            var products = _repository.GetProducts();
            Console.WriteLine("Inventory:");
            foreach (var product in products)
            {
                if (product.Name == edited && !string.IsNullOrEmpty(edited))
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"{product.Name,-25} {product.Price,-20} {product.Quantity,-15}");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine($"{product.Name,-25} {product.Price,-20} {product.Quantity,-15}");
                }
            }
        }

        public void EditProduct()
        {
            Console.Write("Enter the name of the product: ");
            string name = Console.ReadLine();

            var product = _repository.GetProductByName(name);
            if (product == null)
            {
                Console.WriteLine($"The product '{name}' was not found.");
                return;
            }

            Console.Write("Enter the new price: ");
            product.Price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter the new quantity: ");
            product.Quantity = int.Parse(Console.ReadLine());

            _repository.UpdateProduct(product);
            Console.WriteLine("Product updated successfully.");
        }

        public void DeleteProduct()
        {
            Console.Write("Enter the name of the product to delete: ");
            string name = Console.ReadLine();

            _repository.DeleteProduct(name);
            Console.WriteLine($"Product '{name}' deleted successfully.");
        }

        public void SearchProduct()
        {
            Console.Write("Enter the name of the product to search for: ");
            string name = Console.ReadLine();

            var product = _repository.GetProductByName(name);
            if (product == null)
            {
                Console.WriteLine($"The product '{name}' was not found.");
            }
            else
            {
                Console.WriteLine($"Found: Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}");
            }
        }

        private Product GetProductDetails()
        {
            Console.Write("Enter the product name: ");
            var name = Console.ReadLine();
            Console.Write("Enter the product price: ");
            var price = decimal.Parse(Console.ReadLine());
            Console.Write("Enter the product quantity: ");
            var quantity = int.Parse(Console.ReadLine());

            return new Product(name, quantity, price);
        }
    }
}
