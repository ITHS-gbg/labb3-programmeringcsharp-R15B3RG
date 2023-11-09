using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Views;

namespace Labb3ProgTemplate.Managerrs;

public static class UserManager
{
    private static readonly IEnumerable<User>? _users = new List<User>();
    private static User _currentUser;

    public static IEnumerable<User>? Users => _users;

    public static User CurrentUser
    {
        get => _currentUser;
        set
        {
            _currentUser = value;
            CurrentUserChanged?.Invoke();
        }
    }

    public static event Action CurrentUserChanged;

    // Skicka detta efter att användarlistan ändrats eller lästs in
    public static event Action UserListChanged;

    public static bool IsAdminLoggedIn => CurrentUser?.Type == UserTypes.Admin;
    public static bool IsCustomerLoggedIn => CurrentUser?.Type == UserTypes.Customer;

    public static void ChangeCurrentUser(string name, string password)
    {
        // Kontrollera om användaren hittades
        if (((List<User>)Users).Exists(u => u.Name == name))
        {
            var tryLogInUser = ((List<User>)Users).FirstOrDefault(u => u.Name == name);

            // Verifiera lösenordet
            if (tryLogInUser.Authenticate(password))
            {
                // Lösenordet är korrekt, ställ in den aktuella användaren
                UserManager.CurrentUser = tryLogInUser;

            }
            else
            {
                // Lösenordet är felaktigt
                MessageBox.Show("Felaktigt lösenord");
            }
        }
        else
        {
            // Användaren finns inte
            MessageBox.Show("Användaren finns inte");
        }
        CurrentUserChanged?.Invoke();
    }

    public static void LogOut()
    {
        
    }

    public static async Task SaveUsersToFile() //Allt är korrekt i denna kod! RÖR EJ!
    {

        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "UsersJson");
        Directory.CreateDirectory(directory);
        var filepath = Path.Combine(directory, "Users.json");

        // Här konverteras List<User> till IEnumerable<User> innan serialisering
        var jsonOptions = new JsonSerializerOptions
        {
            WriteIndented = true
        };

        // Använd ToList() för att konvertera IEnumerable<User> till List<User>
        var json = JsonSerializer.Serialize(_users.ToList(), jsonOptions);

        using (var sw = new StreamWriter(filepath))
        {
            sw.WriteLine(json);
        };

    }

    public static async Task LoadUsersFromFile() //Allt är korrekt i denna kod! RÖR EJ!
    {

        var directory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "UsersJson");
        var filepath = Path.Combine(directory, "Users.json");

        if (File.Exists(filepath)) //Om Json skapat en sparad fil att läsa ifrån
        {
            var text = string.Empty;
            string? line = string.Empty;

            using var sr = new StreamReader(filepath);

            //sr.ReadToEnd();

            while ((line = sr.ReadLine()) != null)
            {
                text += line;
            }

            var deserialisedUser = new List<User>();
            using (var jsonDoc = JsonDocument.Parse(text))
            {
                if (jsonDoc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (var jsonElement in jsonDoc.RootElement.EnumerateArray())
                    {
                        User a;
                        switch (jsonElement.GetProperty("Type").GetInt32())
                        {
                            case 0:
                                a = jsonElement.Deserialize<Admin>();
                                deserialisedUser.Add(a);
                                break;
                            case 1:

                                a = jsonElement.Deserialize<Customer>();
                                deserialisedUser.Add(a);
                                break;
                        }
                    }
                }

            }

            ((List<User>)_users).AddRange(deserialisedUser);
        }

    }

    
}