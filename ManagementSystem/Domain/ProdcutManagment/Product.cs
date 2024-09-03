using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManagementSystem.Domain.ProdcutManagment
{
    internal class Product
    {
        public string Name { get; set; }
        private int quantity;
        private decimal price;
        public decimal Price
        {
            get => price;
            set 
            {
                if (value <= 0)
                    throw new ArgumentException("The price can not be less than zero!");
                price = value;
            }
        }
        public int Quantity
        {
            get => quantity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("The quantitiy can not be less than zero!");
                quantity = value;
            }
        }
        public Product(string name, int quantity, decimal price)
        {
            this.Name = name;
            this.quantity = quantity;
            this.price = price;
        }
        public override string ToString()
        {
            return $"{Name};{Price};{Quantity}";
        }
    }
}
