using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Entity
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="This Area is Required")]
        public string Name { get; set; }
        public double Price { get; set; }
        public List<ProductCategory> productCategories { get; set; } 

    }
}
