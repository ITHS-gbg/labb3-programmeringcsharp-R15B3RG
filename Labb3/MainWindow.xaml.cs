
using System.Windows;
using Labb3ProgTemplate.Managerrs;
using Labb3ProgTemplate.Views;

namespace Labb3ProgTemplate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ServiceCenter.GetUsers();
            
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;

            ProductManager.LoadProductsFromFile();
        }

        private void UserManager_CurrentUserChanged()
        {
            if (UserManager.IsAdminLoggedIn)
            {
                AdminTab.Visibility = Visibility.Visible;
                ShopTab.Visibility = Visibility.Visible;
                LoginTab.Visibility = Visibility.Hidden;

                AdminTab.IsSelected = true;
                ShopTab.IsSelected = false;
                LoginTab.IsSelected = false;
            }
            else if (UserManager.IsCustomerLoggedIn)
            {
                ShopTab.Visibility = Visibility.Visible;
                AdminTab.Visibility = Visibility.Hidden;
                LoginTab.Visibility = Visibility.Hidden;

                ShopTab.IsSelected = true;
                AdminTab.IsSelected = false;
                LoginTab.IsSelected = false;
            }
            else
            {
                ShopTab.Visibility = Visibility.Hidden;
                AdminTab.Visibility = Visibility.Hidden;
                LoginTab.Visibility = Visibility.Visible;

                LoginTab.IsSelected = true;
                ShopTab.IsSelected = false;
                AdminTab.IsSelected = false;
            }

        }

        private void LoginView_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
