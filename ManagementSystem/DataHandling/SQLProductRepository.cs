using ManagementSystem.Models;
using Microsoft.Data.SqlClient;

namespace ManagementSystem.DataHandling
{
    public class SQLProductRepository(string connectionString)
    {
        private readonly string _connectionString = connectionString;

        public void AddProduct(Product product)
        {
            string query = "INSERT INTO Products (Name, Price, Quantity) VALUES (@Name, @Price, @Quantity)";
            ExecuteNonQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
            });
        }

        public List<Product> GetProducts()
        {
            string query = "SELECT Name, Price, Quantity FROM Products";
            return ExecuteReader(query, null, reader =>
            {
                return new Product(
                    reader["Name"].ToString(),
                    (int)reader["Quantity"],
                    (decimal)reader["Price"]);
            });
        }

        public Product GetProductByName(string name)
        {
            string query = "SELECT Name, Price, Quantity FROM Products WHERE Name = @Name";
            var products = ExecuteReader(query, cmd => cmd.Parameters.AddWithValue("@Name", name), reader =>
            {
                return new Product(
                    reader["Name"].ToString(),
                    (int)reader["Quantity"],
                    (decimal)reader["Price"]);
            });

            return products.Count > 0 ? products[0] : null;
        }

        public void UpdateProduct(Product product)
        {
            string query = "UPDATE Products SET Price = @Price, Quantity = @Quantity WHERE Name = @Name";
            ExecuteNonQuery(query, cmd =>
            {
                cmd.Parameters.AddWithValue("@Name", product.Name);
                cmd.Parameters.AddWithValue("@Price", product.Price);
                cmd.Parameters.AddWithValue("@Quantity", product.Quantity);
            });
        }

        public void DeleteProduct(string name)
        {
            string query = "DELETE FROM Products WHERE Name = @Name";
            ExecuteNonQuery(query, cmd => cmd.Parameters.AddWithValue("@Name", name));
        }

        private void ExecuteNonQuery(string query, Action<SqlCommand> parameterize)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    parameterize?.Invoke(command);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private List<Product> ExecuteReader(string query, Action<SqlCommand> parameterize, Func<SqlDataReader, Product> map)
        {
            var products = new List<Product>();
            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    parameterize?.Invoke(command);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            products.Add(map(reader));
                        }
                    }
                }
            }
            return products;
        }
    }
}
