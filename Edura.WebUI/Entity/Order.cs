using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Entity
{
    public class Order
    {
        public Order()
        {
            OrderLines = new List<OrderLine>();
        }
        public int Id { get; set; }
        public string orderNumber { get; set; }
        public double Total { get; set; }
        public DateTime OrderDate { get; set; }
        public EnumOrderState OrderState { get; set; }
        public string Username { get; set; } // kullanıcı ile ilişki.
        public string AdresBasligi { get; set; }
        public string Adres { get; set; }
        public string Sehir { get; set; }
        public string Semt { get; set; }
        public string Telefon { get; set; }
        public List<OrderLine> OrderLines { get; set; }
    }
    public enum EnumOrderState
    {
        [Display(Name="Order is Waiting")]
        Waiting,
        [Display(Name = "Order is Complated")]
        Complated
    }
}
