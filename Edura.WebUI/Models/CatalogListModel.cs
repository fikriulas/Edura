using Edura.WebUI.Entity;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Models
{
    public class CatalogListModel
    {
        public List<Product> Products { get; set; }        
        public List<Category> Categories { get; set; }        
        public IPagedList<Product> prod { get; set; }
    }    
}
