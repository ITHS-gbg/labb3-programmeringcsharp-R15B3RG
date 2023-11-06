using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for ShopView.xaml
    /// </summary>
    public partial class ShopView : UserControl
    {

        private List<Product> shoppingCart = new List<Product>();

        public ShopView()
        {
            InitializeComponent();
            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;

            ProductManager.LoadProductsFromFile();

            // Initialisera listan över tillgängliga produkter (du behöver hämta detta från någonstans, t.ex. ProductManager)

            UpdateProductList();

        }

        private void UserManager_CurrentUserChanged()
        {

        }

        private void UpdateProductList()
        {
            // Uppdatera listan över tillgängliga produkter i UI
            ProdList.Items.Clear();

            foreach (var product in ProductManager.Products)
            {
                ProdList.Items.Add($"{product.Name}  -  {product.Price:C2}");
            }

        }

        private void UpdateShoppingCart()
        {
            // Clear existing items in the shopping cart list
            CartList.Items.Clear();

            // Add the updated items to the shopping cart list
            foreach (var product in shoppingCart)
            {
                CartList.Items.Add($"{product.Name} - {product.Price:C2}");
            }


        }

        private void RemoveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is string selectedProductString)
            {
                var productName = selectedProductString.Split('-')[0].Trim();
                var selectedProduct = shoppingCart.FirstOrDefault(p => p.Name == productName);

                if (selectedProduct != null)
                {
                    // Ta bort produkten från kundvagnen
                    shoppingCart.Remove(selectedProduct);

                    // Uppdatera visningen av kundvagnen i UI
                }
            }

            UpdateShoppingCart();
        }

        private void AddBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is string selectedProductString)
            {
                var productName = selectedProductString.Split('-')[0].Trim();
                var selectedProduct = ProductManager.Products.FirstOrDefault(p => p.Name == productName);

                if (selectedProduct != null)
                {
                    // Klona produkten och lägg till den i kundvagnen
                    shoppingCart.Add(new NewProduct(selectedProduct.Name, selectedProduct.Price, ProductTypes.NewProducts));

                    UpdateShoppingCart();
                }
            }
        }

        private void LogoutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Logga ut användaren genom att sätta CurrentUser till null
            UserManager.CurrentUser = null;

            // Visa LoginView efter utloggning
            Content = new LoginView();
        }

        private void CheckoutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (UserManager.CurrentUser != null)
            {
                // Anropa Checkout-metoden i ProductManager
                ProductManager.Checkout(UserManager.CurrentUser, shoppingCart);

                // Töm kundvagnen och uppdatera gränssnittet
                shoppingCart.Clear();
                UpdateShoppingCart();
            }
        }
    }
}
