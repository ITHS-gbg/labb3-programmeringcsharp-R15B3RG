using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Linq;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.DataModels.Users;
using static System.Net.Mime.MediaTypeNames;

namespace Labb3ProgTemplate.Managerrs;

public static class ProductManager
{
    private static readonly IEnumerable<Product>? _products = new List<Product>();
    public static IEnumerable<Product>? Products => _products;


    public static event Action ProductListChanged;

    



    public static void AddProduct(Product product) //Allt är korrekt i denna kod! RÖR EJ!
    {

        if (((List<Product>)Products).Exists(p => product.Name == ""))
        {
            return;
        }
        else
        {
            ((List<Product>)_products).Add(product);
            ProductListChanged?.Invoke();
        }
    }

    public static void RemoveProduct(Product product)
    {

        ((List<Product>)_products).Remove(product);
        ProductListChanged?.Invoke();

    }



    public static void Checkout(User currentUser, List<Product> shoppingCart)
    {
        
    }





    public static async Task SaveProductsToFile()
    {

        // Bestämmer mappen och filens sökväg för att spara användarna.
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProductsJson");
        Directory.CreateDirectory(directory); // Skapar mappen om den inte finns.
        var filepath = Path.Combine(directory, "Products.json");

        // Inställningar för hur JSON ska serialiseras.
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true // Formatet är indenterat för läsbarhet.
        };

        // Serialiserar användarlistan till en JSON-sträng.
        var json = JsonSerializer.Serialize(_products.ToList(), jsonOptions);

        // Skriver JSON-strängen till filen.
        using (var sw = new StreamWriter(filepath))
        {
            sw.WriteLine(json);
        }

    }

    public static async Task LoadProductsFromFile()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "ProductsJson");
        Directory.CreateDirectory(directory);

        // Bestämmer filvägen till "Users.json" inom mappen "UsersJson".
        var filepath = Path.Combine(directory, "Products.json");

        // Kontrollerar om filen "Users.json" finns. Om den inte finns, avsluta metoden.
        if (!File.Exists(filepath))
        {
            return;
        }

        // Initierar en tom sträng för att hålla JSON-innehållet från filen.
        var json = string.Empty;

        // Öppnar filen "Users.json" för läsning och läser hela dess innehåll till variabeln 'json'.
        using (var sr = new StreamReader(filepath))
        {
            json = sr.ReadToEnd();
        }

        // Deserialisera JSON-innehållet till en temporär lista
        var deserializedProducts = JsonSerializer.Deserialize<List<NewProduct>>(json);

        // Rensa den befintliga produklistan innan du lägger till de deserialiserade produkterna
        ((List<Product>)_products).Clear();

        // Initierar en ny lista för att hålla deserialiserade User-objekt.
        ((List<Product>)_products).AddRange(JsonSerializer.Deserialize<List<NewProduct>>(json));

        
        ProductListChanged.Invoke();
    }

}
