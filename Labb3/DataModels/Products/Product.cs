using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate.DataModels.Products;

public abstract class Product 
{
    public string Name { get; set; }
   
    public double Price { get; set; }
    

    public ProductTypes Type { get; }

    protected Product(string name, double price, ProductTypes type)
    {
        Name = name;
        Price = price;
        Type = type;
    }
}