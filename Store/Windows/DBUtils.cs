using Store.StoreModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.Windows
{
    public class DBUtils
    {
        /// <summary>
        /// Dodawanie produktu do bazy
        /// </summary>
        public static bool addProduct(StoreDBEntities db, Category c, string name, double price)
        {
            try
            {
                Product p = new Product();
                p.name = name;
                p.price = price;
                p.Category_Id = c.Category_Id;
                db.Products.Add(p);

                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Dodawanie zamównienia do bazy
        /// </summary>
        public static bool addPurchase(StoreDBEntities db, Customer c, Product p)
        {
            if (c == null || p == null)
                return false;

            try
            {

                Purchase pr = new Purchase();
                pr.Customer_Id = c.Customer_Id;
                pr.Product_Id = p.Product_Id;
                pr.data = DateTime.Now;
                db.Purchases.Add(pr);
                db.SaveChanges();
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }
    }
}
