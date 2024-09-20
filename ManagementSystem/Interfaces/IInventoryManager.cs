using ManagementSystem.Models;

namespace ManagementSystem.Interfaces
{
    public interface IInventoryManager
    {
        public void AddProduct();
        public void ListProducts(string edited);
        public void EditProduct();
        public void DeleteProduct();
        public void SearchProduct();
    }
}
