using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
        }

        private void UserManager_CurrentUserChanged()
        {
            if (UserManager.CurrentUser != null)
            {
                // Använd den aktuella användaren för att bestämma vilken vy som ska visas
                if (UserManager.CurrentUser.Type == UserTypes.Admin)
                {
                    // Visa AdminView när en Admin loggar in
                    Content = new AdminView();
                }
                else if (UserManager.CurrentUser.Type == UserTypes.Customer)
                {
                    // Visa ShopView när en Customer loggar in
                    Content = new ShopView();
                }
            }
            else
            {
                // Visa LoginView om ingen användare är inloggad
                Content = new LoginView();
            }
        }

        private void LoginBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Hämta användarnamn och lösenord från TextBox-kontroller
            string username = LoginName.Text;
            string password = LoginPwd.Password;

            UserManager.ChangeCurrentUser(username, password);

        }

        private void RegisterAdminBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Hämta användarnamn och lösenord från TextBox-kontroller
            string username = RegisterName.Text;
            string password = RegisterPwd.Password;

            // Skapa en ny Admin
            Admin newAdmin = new Admin(username, password);

            // Lägg till nya Admin i användarlistan
            ((List<User>)UserManager.Users).Add(newAdmin);

            // Spara användarna till fil
            UserManager.SaveUsersToFile();

        }

        private void RegisterCustomerBtn_OnClickmerBtn_Click(object sender, RoutedEventArgs e)
        {
            // Hämta användarnamn och lösenord från TextBox-kontroller
            string username = RegisterName.Text;
            string password = RegisterPwd.Password;

            Customer newCustomer = new Customer(username, password);

            ((List<User>)UserManager.Users).Add(newCustomer);

            UserManager.SaveUsersToFile();

            
        }
    }
}
