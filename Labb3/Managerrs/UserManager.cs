using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;

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

    public static bool IsAdminLoggedIn => CurrentUser.Type is UserTypes.Admin;

    public static void ChangeCurrentUser(string name, string password, UserTypes type)
    {
        throw new NotImplementedException();
    }

    public static void LogOut()
    {
        throw new NotImplementedException();
    }

    public static async Task SaveUsersToFile()
    {
        throw new NotImplementedException();
    }

    public static async Task LoadUsersFromFile()
    {
        throw new NotImplementedException();
    }
}