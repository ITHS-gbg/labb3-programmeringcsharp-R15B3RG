using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using Labb3ProgTemplate.DataModels.Products;
using Labb3ProgTemplate.DataModels.Users;
using Labb3ProgTemplate.Enums;
using Labb3ProgTemplate.Managerrs;

namespace Labb3ProgTemplate.Views
{

    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : UserControl
    {


        public AdminView()
        {
            InitializeComponent();

            UserManager.CurrentUserChanged += UserManager_CurrentUserChanged;
            ProductManager.ProductListChanged += UpdateProductList;

            ProductManager.LoadProductsFromFile();
            
            UpdateProductList();
        }
        

        private void UpdateProductList()
        {

            ProdList.Items.Clear();

            foreach (var product in ProductManager.Products)
            {
                ProdList.Items.Add($"{product.Name} - {product.Price:C2}");
            }
        }

        private void UserManager_CurrentUserChanged()
        {

        }
        
        private void ProdList_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void SaveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {

            string productName = NameTextBox.Text;

            // Kontrollera om både namn och pris har angetts
            if (double.TryParse(PriceTextBox.Text, out double productPrice))
            {
                // Skapa en ny instans av NewProduct
                NewProduct newProduct = new NewProduct(productName, productPrice, ProductTypes.NewProducts);

                // Lägg till den nya produkten i listan
                ProdList.Items.Add($"{newProduct.Name} - {newProduct.Price:C2}");

                ProductManager.AddProduct(newProduct);

                ProductManager.SaveProductsToFile();

                UpdateProductList();
            }
            else
            {
                // Visa ett meddelande om ogiltig inmatning
                MessageBox.Show("Var god och mata in ett pris med siffror!");
            }

            

        }

        private void RemoveBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var selectedProductString = ProdList.SelectedItem as string;

            if (selectedProductString != null)
            {
                // Split the selected string to get the product name.
                var productName = selectedProductString.Split('-')[0].Trim();

                // Find the product in the product list by its name.
                var productToRemove = ProductManager.Products?.FirstOrDefault(p => p.Name == productName);

                if (productToRemove != null)
                {
                    // Remove the product from the ProductManager.
                    ProductManager.RemoveProduct(productToRemove);

                    // Remove the product from the ProdList.
                    ProdList.Items.Remove(selectedProductString);

                    // Update the selection (if there are items left).
                    if (ProdList.Items.Count > 0)
                    {
                        ProdList.SelectedIndex = 0;
                    }

                    // Optional: You might want to save the changes to the file.
                    ProductManager.SaveProductsToFile();

                    UpdateProductList();
                }
            }
            else
            {
                MessageBox.Show("Var god och välj en giltig vara att ta bort.");
            }

        }

        private void LogoutBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Logga ut användaren genom att sätta CurrentUser till null
            UserManager.CurrentUser = null;

            // Visa LoginView efter utloggning
            Content = new LoginView();
        }
    }
}
