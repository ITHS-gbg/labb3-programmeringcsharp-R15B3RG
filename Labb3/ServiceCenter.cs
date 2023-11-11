using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Windows;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;

namespace Labb3ProgTemplate;

public class ServiceCenter
{
    private const string UserJsonDirectory = "UsersJson";
    private const string UserJsonFile = "Users.json";

    private static string CreateDirectoryAndGetPath()
    {
        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), UserJsonDirectory);
        Directory.CreateDirectory(directory);
        return Path.Combine(directory, UserJsonFile);
    }

    public static void AddUser(string username, string password, UserTypes usertype)
    {
        // Skapa ny användare
        User? newUser = null;
        switch (usertype)
        {
            case UserTypes.Admin:
                newUser = new Admin(username, password);
                break;
            case UserTypes.Customer:
                newUser = new Customer(username, password);
                break;
        }
        if (newUser == null)
        {
            MessageBox.Show("New user var null");
            return;
        }

        // Hämta aktuella användare
        List<User> currentUsers = GetUsers() ?? new List<User>();

        // Om användaren med detta namn redan finns. STOP!
        if (currentUsers.FirstOrDefault(x => x.Name.ToLower().Equals(username.ToLower())) != null)
        {
            MessageBox.Show("Namnet " + username + " finns redan");
            return;
        }
        else if (currentUsers.FirstOrDefault(x => x.Name.ToLower().Equals(username.ToLower())) == null)
        {
            MessageBox.Show("Ny " + usertype + " skapad!");
        }

        // Lägg till nya användaren i aktuell lista
        currentUsers.Add(newUser);

        if (newUser is Customer customer)
        {
            customer.SelectedProducts = new List<NewProduct>();
        }
        // Skapa ny json för den nya användaren
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };
        var json = JsonSerializer.Serialize(currentUsers.ToList(), jsonOptions);
        var filepath = CreateDirectoryAndGetPath();
        using (var streamWriter = new StreamWriter(filepath))
        {
            streamWriter.WriteLine(json);
        };

    }


    public static List<User>? GetUsers()
    {
        // Hämta sökväg till json filen
        var filepath = CreateDirectoryAndGetPath();

        // Finns den INTE, så stoppar vi koden här.
        if (!File.Exists(filepath)) return null;

        // Skapa json sträng från JSON-filen
        var jsonString = string.Empty;
        string? eachLine = string.Empty;
        using var streamReader = new StreamReader(filepath);
        while ((eachLine = streamReader.ReadLine()) != null) jsonString += eachLine;
        
        // Skapa lista from json till C#
        var deserialisedUsers = new List<User>();
        using (var jsonObject = JsonDocument.Parse(jsonString))
        {
            // Om json inte är en lista, stoppa skiten nu!
            if (jsonObject.RootElement.ValueKind != JsonValueKind.Array) return null;
            
            // Loopar vi igenom varje object i json arraren
            foreach (JsonElement jsonElement in jsonObject.RootElement.EnumerateArray())
            {
                // Lägger till alla användare i en array..som INTE ÄR NULL!
                User currentUser = jsonElement.GetCurrentUser();
                if (currentUser == null) continue;
                deserialisedUsers.Add(currentUser);
            }
        }
        return deserialisedUsers;
    }




    public static void SaveShoppingCart(User user)
    {
        if (user.Type == UserTypes.Customer)
        {
            var customer = (Customer)user;
            var cartJson = JsonSerializer.Serialize(customer.SelectedProducts);

            // Spara kundvagnen till en separat fil för varje användare
            var cartFilePath = GetCartFilePath(user.Name);
            File.WriteAllText(cartFilePath, cartJson);
        }
    }


    public static List<NewProduct> LoadShoppingCart(User user)
    {
        if (user != null && user.Type == UserTypes.Customer)
        {
            var cartFilePath = GetCartFilePath(user.Name);

            if (File.Exists(cartFilePath))
            {
                var cartJson = File.ReadAllText(cartFilePath);
                return JsonSerializer.Deserialize<List<NewProduct>>(cartJson) ?? new List<NewProduct>();
            }
        }

        return new List<NewProduct>();
    }

    private static string GetCartFilePath(string username)
    {
        var cartDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Carts");
        Directory.CreateDirectory(cartDirectory);
        return Path.Combine(cartDirectory, $"{username}_Cart.json");
    }
}



public static class MinaExtensions
{
    public static User? GetCurrentUser(this JsonElement jsonElement)
    {
        var type = jsonElement.GetProperty("Type").GetInt32();
        return type switch
        {
            0 => jsonElement.Deserialize<Admin>(),
            1 => jsonElement.Deserialize<Customer>(),
            _ => null
        };
    }
}

