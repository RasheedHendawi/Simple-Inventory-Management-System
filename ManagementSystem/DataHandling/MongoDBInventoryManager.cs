using ManagementSystem.Interfaces;
using ManagementSystem.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace ManagementSystem.DataHandling
{
    public class MongoDBInventoryManager : IInventoryManager
    {
        private readonly IMongoCollection<Product> _productCollection;

        public MongoDBInventoryManager(IConfiguration configuration)
        {
            var connectionString = configuration["MongoDB:ConnectionString"];
            var databaseName = configuration["MongoDB:DatabaseName"];
            var collectionName = configuration["MongoDB:CollectionName"];
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _productCollection = database.GetCollection<Product>(collectionName);
        }

        public void AddProduct()
        {
            var product = GetProductDetails();
            if (product != null)
            {
                _productCollection.InsertOne(product);
                Console.WriteLine("Product added successfully!");
            }
        }

        public void ListProducts(string edited)
        {
            var products = _productCollection.Find(_ => true).ToList();
            Console.WriteLine("Inventory\n");
            Console.WriteLine("{0,-25} {1,-20} {2,-15}", "Name", "Price", "Quantity");

            foreach (var product in products)
            {
                if (!string.IsNullOrEmpty(edited) && product.Name == edited)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("{0,-25} {1,-20} {2,-15}", product.Name, product.Price, product.Quantity);
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("{0,-25} {1,-20} {2,-15}", product.Name, product.Price, product.Quantity);
                }
            }
        }

        public void EditProduct()
        {
            Console.Write("Enter the name of the product to edit: ");
            var name = Console.ReadLine();
            var product = _productCollection.Find(p => p.Name == name).FirstOrDefault();

            if (product == null)
            {
                Console.WriteLine($"The product '{name}' was not found.");
                return;
            }

            Console.WriteLine($"Editing product: {product.Name}");
            Console.Write("New Name (Leave blank to keep unchanged): ");
            var newName = Console.ReadLine();

            Console.Write("New Price (Leave blank to keep unchanged): ");
            var newPriceInput = Console.ReadLine();

            Console.Write("New Quantity (Leave blank to keep unchanged): ");
            var newQuantityInput = Console.ReadLine();

            if (!string.IsNullOrEmpty(newName)) product.Name = newName;
            if (decimal.TryParse(newPriceInput, out var newPrice) && newPrice > 0) product.Price = newPrice;
            if (int.TryParse(newQuantityInput, out var newQuantity) && newQuantity >= 0) product.Quantity = newQuantity;

            _productCollection.ReplaceOne(p => p.Name == name, product);
            Console.WriteLine("Product updated successfully!");
        }

        public void DeleteProduct()
        {
            Console.Write("Enter the name of the product to delete: ");
            var name = Console.ReadLine();

            var result = _productCollection.DeleteOne(p => p.Name == name);

            if (result.DeletedCount > 0)
            {
                Console.WriteLine($"The product '{name}' was deleted successfully!");
            }
            else
            {
                Console.WriteLine($"The product '{name}' was not found.");
            }
        }

        public void SearchProduct()
        {
            Console.Write("Enter the name of the product to search: ");
            var name = Console.ReadLine();

            var product = _productCollection.Find(p => p.Name == name).FirstOrDefault();

            if (product != null)
            {
                Console.WriteLine("Product found:");
                Console.WriteLine($"Name: {product.Name}, Price: {product.Price}, Quantity: {product.Quantity}");
            }
            else
            {
                Console.WriteLine($"The product '{name}' was not found.");
            }
        }

        private Product? GetProductDetails()
        {
            Console.Write("Enter the product name: ");
            var name = Console.ReadLine();

            Console.Write("Enter the product price: ");
            if (!decimal.TryParse(Console.ReadLine(), out var price) || price <= 0)
            {
                Console.WriteLine("Invalid price. Please enter a valid decimal number greater than zero.");
                return null;
            }

            Console.Write("Enter the product quantity: ");
            if (!int.TryParse(Console.ReadLine(), out var quantity) || quantity < 0)
            {
                Console.WriteLine("Invalid quantity. Please enter a valid non-negative integer.");
                return null;
            }

            return new Product(name, quantity, price);
        }
    }
}
