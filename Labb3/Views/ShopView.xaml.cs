using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ObjectiveC;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{
    /// <summary>
    /// Interaction logic for ShopView.xaml
    /// </summary>
    public partial class ShopView : UserControl
    {

        private List<NewProduct> shoppingCart;

        public ShopView()
        {
            InitializeComponent();

            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += ProductManager_ProductListChanged;

            ProductManager.LoadProductsFromFile();

            // Initialisera listan över tillgängliga produkter (du behöver hämta detta från någonstans, t.ex. ProductManager)

            UpdateProductList();

        }

        private void ProductManager_ProductListChanged()
        {
            // Uppdatera listan över tillgängliga produkter när förändringar inträffar
            UpdateProductList();
        }

        private void UserManager_CurrentUserChanged()
        {
            if (UserManager.CurrentUser != null)
            {
                shoppingCart = UserManager.CurrentUser.Cart;

                if (UserManager.CurrentUser is Customer customer)
                {

                    UserManager.LoadUsersFromFile();

                    foreach (var product in UserManager.CurrentUser.Cart)

                    {

                        CartList.Items.Add($"{product.Name} - {product.Price:C2}");
                    }
                }

                // Uppdatera gränssnittet efter att ha laddat kundvagnen
                UpdateShoppingCart();
            }


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
            var selectedProductString = CartList.SelectedItem as string;

            if (selectedProductString != null)
            {
                // Split the selected string to get the product name.
                var productName = selectedProductString.Split('-')[0].Trim();

                // Find the product in the shoppingCart by its name.
                var productToRemove = shoppingCart.FirstOrDefault(p => p.Name == productName);

                if (productToRemove != null)
                {
                    // Remove the product from the shoppingCart.
                    shoppingCart.Remove(productToRemove);

                    // Remove the product from the CartList.
                    CartList.Items.Remove(selectedProductString);

                    
                    // Update the selection (if there are items left).
                    if (CartList.Items.Count > 0)
                    {
                        CartList.SelectedIndex = 0;
                    }

                }

                UpdateShoppingCart();
            }
            else
            {
                MessageBox.Show("Var god och välj en giltig vara att ta bort.");
            }
        }

        private void AddBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (ProdList.SelectedItem is string selectedProductString)
            {
                var productName = selectedProductString.Split('-')[0].Trim();
                var selectedProduct = ProductManager.Products.FirstOrDefault(p => p.Name == productName) as NewProduct;

                if (selectedProduct != null)
                {
                    shoppingCart.Add(selectedProduct);
                    UpdateShoppingCart();
                }
            }
        }

        private void LogoutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            // Logga ut användaren genom att sätta CurrentUser till null
            UserManager.CurrentUser = null;


        }

        private void CheckoutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (UserManager.CurrentUser != null)
            {
                // Beräkna det totala priset innan kundvagnen töms
                var totalCostBeforeCheckout = shoppingCart.Sum(p => p.Price);

                // Anropa Checkout-metoden i ProductManager
                ProductManager.Checkout(UserManager.CurrentUser, shoppingCart);

                // Beräkna det totala priset efter kassan
                var totalCostAfterCheckout = shoppingCart.Sum(p => p.Price);

                // Beräkna prisskillnaden
                var priceDifference = totalCostBeforeCheckout - totalCostAfterCheckout;

                // Visa en MessageBox med den totala kostnaden och prisskillnaden.
                MessageBox.Show($"Totalt pris före kassan: {totalCostBeforeCheckout:C2}\nTotalt pris efter kassan: {totalCostAfterCheckout:C2}\nPrisskillnad: {priceDifference:C2}\nTack för att du handlade hos oss!");

                // Töm kundvagnen och uppdatera gränssnittet
                shoppingCart.Clear();
                UpdateShoppingCart();
            }
        }

    }
}


