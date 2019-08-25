using Edura.WebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Models
{
    public class OrderComplateModel
    {
        public List<OrderLine> OrderLine { get; set; }
        public Order Order { get; set; }
        public Product Product { get; set; }
    }
}
