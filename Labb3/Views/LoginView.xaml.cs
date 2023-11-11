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
            var users = ServiceCenter.GetUsers();

            if (users != null && UserManager.CurrentUser != null)
            {
                var currentUser = users.FirstOrDefault(u => u.Name == UserManager.CurrentUser.Name);

                if (currentUser != null)
                {
                    if (currentUser.Type == UserTypes.Admin)
                    {
                        // Visa AdminView när en Admin loggar in
                        Content = new AdminView();
                    }
                    else if (currentUser.Type == UserTypes.Customer)
                    {
                        // Visa ShopView när en Customer loggar in
                        Content = new ShopView();
                    }

                    // Ingen behov av att gå vidare eftersom vi har redan ställt in rätt vy
                    return;
                }
            }

            // Visa LoginView om ingen användare är inloggad eller om användaren inte hittades
            Content = new LoginView();
        }

        private void LoginBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Hämta användarnamn och lösenord från TextBox-kontroller
            string username = LoginName.Text;
            string password = LoginPwd.Password;
            if (username is "" or null)
            {
                MessageBox.Show("Något gick fel");
                return;
            }

            UserManager.ChangeCurrentUser(username, password);

            var users = ServiceCenter.GetUsers();
            if (users == null) return;

            var currentUser = users.FirstOrDefault(x => x.Name.ToLower().Equals(username.ToLower()) && x.Password.Equals(password));
            if (currentUser == null)
            {
                MessageBox.Show("Lösenordet är felaktigt");
                return;
            }

        }

        private void RegisterAdminBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            ServiceCenter.AddUser(
                RegisterName.Text,
                RegisterPwd.Password,
                UserTypes.Admin
                );

            
            ClearRegistrationFields();

            
        }

        private void RegisterCustomerBtn_OnClickmerBtn_Click(object sender, RoutedEventArgs e)
        {
            ServiceCenter.AddUser(
                RegisterName.Text,
                RegisterPwd.Password,
                UserTypes.Customer
            );
            

            ClearRegistrationFields();

        }

        private void ClearRegistrationFields()
        {
            // Rensa TextBox-kontrollerna
            RegisterName.Clear();
            RegisterPwd.Clear();
        }
    }
}
