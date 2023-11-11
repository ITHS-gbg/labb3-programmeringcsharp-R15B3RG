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
    public static event Action ShoppingCartChanged;


    public static bool IsAdminLoggedIn => CurrentUser?.Type == UserTypes.Admin;
    public static bool IsCustomerLoggedIn => CurrentUser?.Type == UserTypes.Customer;

    public static void ChangeCurrentUser(string name, string password)
    {
        // Hämta användare från ServiceCenter
        var tryLogInUser = ServiceCenter.GetUsers()?.FirstOrDefault(u => u.Name == name);

        // Kontrollera om användaren hittades
        if (tryLogInUser != null)
        {
            // Verifiera lösenordet
            if (tryLogInUser.Authenticate(password))
            {
                // Lösenordet är korrekt, ställ in den aktuella användaren
                UserManager.CurrentUser = tryLogInUser;

                // Ladda kundvagnen när en kund loggar in
                if (tryLogInUser.Type == UserTypes.Customer)
                {
                    var customer = (Customer)tryLogInUser;
                    customer.SelectedProducts = ServiceCenter.LoadShoppingCart(tryLogInUser);
                }
            }

            CurrentUserChanged?.Invoke();

            ShoppingCartChanged.Invoke();
        }
    }

    public static void LogOut()
    {

    }

}