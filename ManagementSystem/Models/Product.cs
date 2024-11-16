﻿using System;

namespace ManagementSystem.Models
{
    public class Product
    { 
        private int _quantity;
        private decimal _price;
        public Product(string name, int quantity, decimal price)
        {
            Name = name;
            Quantity = quantity;
            Price = price;
        }

        public string Name { get; set; }
        public decimal Price
        {
            get => _price;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("The price can not be less than or equal to zero!");
                _price = value;
            }
        }
        public int Quantity
        {
            get => _quantity;
            set
            {
                if (value <= 0)
                    throw new ArgumentException("The quantitiy can not be less than zero!");
                _quantity = value;
            }
        }
        public override string ToString()
        {
            return $"{Name};{Price};{Quantity}";
        }
    }
}
