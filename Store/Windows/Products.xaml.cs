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
    /// Logika interakcji dla klasy Products.xaml
    /// </summary>
    public partial class Products : Window
    {
        StoreDBEntities db = new StoreDBEntities();
        public Products()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var categories = from c in db.Categories select c;

            Category_Grid.ItemsSource = categories.ToList();
            Category_Grid.Columns[2].Visibility = Visibility.Collapsed;
        }

        private void Category_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Refresh_Products();
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

        private void Click_Add_product(object sender, RoutedEventArgs e)
        {
            try
            {
                Category selCat = (Category)Category_Grid.SelectedItem;
                if (selCat == null)
                    return;

                DBUtils.addProduct(db, selCat, Product_Name.Text, Double.Parse(Product_Price.Text));
                if (Product_Name.Text == "")
                    throw new ArgumentNullException();

                Refresh_Products();
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
                Product selProd = (Product)Product_Grid.SelectedItem;
                db.Products.Remove(selProd);
                db.SaveChanges();
                Refresh_Products();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Click_Update_product(object sender, RoutedEventArgs e)
        {
            try
            {
                Product selProd = (Product)Product_Grid.SelectedItem;
                selProd.name = Product_Name.Text;
                selProd.price = Double.Parse(Product_Price.Text);
                if (Product_Name.Text == "")
                    throw new ArgumentNullException();
                else 
                {
                    db.SaveChanges();
                    Refresh_Products();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Product_Grid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                Product selProd = (Product)Product_Grid.SelectedItem;
                if (selProd == null)
                {
                    Product_Name.Text = "";
                    Product_Price.Text = "";
                    return;
                }
                Product_Name.Text = selProd.name;
                Product_Price.Text = "" + selProd.price;
            }
            catch (Exception)
            {
                Product_Name.Text = "";
                Product_Price.Text = "";
            }
        }
    }
}
