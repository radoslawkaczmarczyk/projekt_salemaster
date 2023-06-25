using Store.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Store.Windows
{
    /// <summary>
    /// Logika interakcji dla klasy Orders.xaml
    /// </summary>
    public partial class Orders : Window
    {
        StoreDBEntities db = new StoreDBEntities();
        public Orders()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var categories = from c in db.Categories select c;

            Category_Grid.ItemsSource = categories.ToList();
            Category_Grid.Columns[2].Visibility = Visibility.Collapsed;

            var customers = from c in db.Customers select c;

            Customer_Grid.ItemsSource = customers.ToList();
            Customer_Grid.Columns[3].Visibility = Visibility.Collapsed;
        }

        void Refresh_Products()
        {
            try
            {
                Category selCat = (Category)Category_Grid.SelectedItem;

                var products = from p in db.Products
                               where p.Category_Id == selCat.Category_Id
                               select p;

                Product_Grid.ItemsSource = products.ToList();

                Product_Grid.Columns[2].Visibility = Visibility.Collapsed;
                Product_Grid.Columns[4].Visibility = Visibility.Collapsed;
                Product_Grid.Columns[5].Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Category_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh_Products();
        }

        class PurchaseData
        {
            public int Purchase_Id { get; set; }
            public DateTime data { get; set; }
            public string name { get; set; }
            public double price { get; set; }
        }

        void Refresh_Purchases()
        {
            try
            {
                Customer selCustomer = (Customer)Customer_Grid.SelectedItem;


                var customer_purchases = from purchase in db.Purchases
                                         join product in db.Products
                                         on purchase.Product_Id equals product.Product_Id
                                         where purchase.Customer_Id == selCustomer.Customer_Id
                                         select new PurchaseData
                                         {
                                             Purchase_Id = purchase.Purchase_Id,
                                             data = purchase.data,
                                             name = product.name,
                                             price = product.price
                                         };

                Purchase_Grid.ItemsSource = customer_purchases.ToList();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Customer_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh_Purchases();
        }

        private void Click_Add_product(object sender, RoutedEventArgs e)
        {
            try
            {
                Customer selCustomer = (Customer)Customer_Grid.SelectedItem;
                Product selProd = (Product)Product_Grid.SelectedItem;

                DBUtils.addPurchase(db, selCustomer, selProd);

                Refresh_Purchases();
            }
            catch (Exception ex) 
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void Click_Delete_product(object sender, RoutedEventArgs e)
        {
            try
            {
                PurchaseData purchaseData = (PurchaseData)Purchase_Grid.SelectedItem;

                Purchase p = db.Purchases.FirstOrDefault(x => x.Purchase_Id == purchaseData.Purchase_Id);
                db.Purchases.Remove(p);
                db.SaveChanges();
                Refresh_Purchases();
            }
            catch (Exception) { }


        }
    }
}
