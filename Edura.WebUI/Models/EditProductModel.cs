using Edura.WebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Models
{
    public class EditProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string Image { get; set; }
        public string Description { get; set; }
        public string HtmlContent { get; set; }

        public bool IsAppproved { get; set; }
        public bool IsHome { get; set; }
        public bool IsFeatures { get; set; }
        public List<AdminEditProductAttribute> Attributes { get; set; }

        public List<AdminEditProductCategory> Category { get; set; }
        public List<AdminEditImageGallery> Images { get; set; }
        public List<AdminEditReview> Reviews { get; set; }

    }
    public class AdminEditProductCategory
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
    }
    public class AdminEditProductAttribute
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Attribute { get; set; }
        public string Value { get; set; }
    }
    public class AdminEditImageGallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class AdminEditReview
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Content { get; set; }
        public int Point { get; set; }
        public DateTime Date { get; set; }
        public int ProductId { get; set; }
        
    }
}
