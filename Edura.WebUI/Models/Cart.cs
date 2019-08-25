using Edura.WebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Models
{
    public class Cart
    {
        private List<CartLine> products =  new List<CartLine>();
        public List<CartLine> Products => products; // sadece get methodudur.

        public void AddProduct(Product product,int quantity)
        {
            var prd = products.Where(i => i.Product.Id == product.Id).FirstOrDefault(); // var olan listeden çağırır.
            if(prd==null)  // ürün yoksa yeni cart ekle.
            {
                products.Add(new CartLine()
                {
                    Product = product,
                    Quantity = quantity
                });
                
            }
            else
            {
                prd.Quantity += quantity;
            }
        }

        public void RemoveProduct(Product product)
        {
            products.RemoveAll(i => i.Product.Id == product.Id);
        }

        public double TotalPrice()
        {
            return products.Sum(i => i.Product.Price * i.Quantity);
        }

        public void ClearAll()
        {
            products.Clear();
        }
    }
    public class CartLine
    {
        public int CartLineId { get; set; }
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
