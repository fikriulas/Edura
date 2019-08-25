using Edura.WebUI.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Repository.Concrete.EntityFramework
{
    public static class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app) //Configure/IApplicationBuilder için.
        {
            var context = app.ApplicationServices.GetRequiredService<EduraContext>();
            context.Database.Migrate(); // otomatik migration yapar.
            if(!context.Products.Any()) // daha önceden bu işlem yapılmış mı?
            {
                var products = new[]
                {
                    new Product(){Name="Samsung S6",Price=1000,Image="product1_thumb.jpg",IsHome=true,IsAppproved=true,IsFeatures=true,Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eu consequat magna, nec dignissim justo.",HtmlContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eu consequat magna, <b>nec dignissim justo.</b>",DateAdded=DateTime.Now.AddDays(-10)},
                    new Product(){Name="Nikon Sg89",Price=9856,Image="product2_thumb.jpg",IsHome=true,IsAppproved=true,IsFeatures=false,Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eu consequat magna, nec dignissim justo.",HtmlContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eu consequat magna, <b>nec dignissim justo.</b>",DateAdded=DateTime.Now.AddDays(-5)},
                    new Product(){Name="Nike Airbin",Price=750,Image="product3_thumb.jpg",IsHome=true,IsAppproved=true,IsFeatures=false,Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eu consequat magna, nec dignissim justo.",HtmlContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eu consequat magna, <b>nec dignissim justo.</b>",DateAdded=DateTime.Now.AddDays(-3)},
                    new Product(){Name="Atasay SunGlass",Price=235,Image="product4_thumb.jpg",IsHome=true,IsAppproved=true,IsFeatures=true,Description="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eu consequat magna, nec dignissim justo.",HtmlContent="Lorem ipsum dolor sit amet, consectetur adipiscing elit. Proin eu consequat magna, <b>nec dignissim justo.</b>",DateAdded=DateTime.Now.AddDays(-85)}
                };
                context.Products.AddRange(products);

                var categories = new[]
                {
                    new Category(){Name="Telefon"},
                    new Category(){Name="Kamera"},
                    new Category(){Name="Ayakkabı"},
                    new Category(){Name="Gözlük"},
                };
                context.Categories.AddRange(categories);

                var productcategories = new[]
                {
                    new ProductCategory(){Product=products[0],Category=categories[0]},
                    new ProductCategory(){Product=products[1],Category=categories[1]},
                    new ProductCategory(){Product=products[2],Category=categories[2]},
                    new ProductCategory(){Product=products[3],Category=categories[3]},
                };
                context.AddRange(productcategories);

                var images = new[]
                {
                    new Image(){Name="t1.jpg",Product = products[0]},
                    new Image(){Name="t2.jpg",Product = products[0]},
                    new Image(){Name="c1.jpg",Product = products[1]},
                    new Image(){Name="c2.jpg",Product = products[1]},
                    new Image(){Name="a1.jpg",Product = products[2]},
                    new Image(){Name="a2.jpg",Product = products[2]},
                    new Image(){Name="a3.jpg",Product = products[2]},
                    new Image(){Name="g1.jpg",Product = products[3]},
                    new Image(){Name="g2.jpg",Product = products[3]},
                    new Image(){Name="g3.jpg",Product = products[3]}
                };
                context.Images.AddRange(images);

                var attribute = new[]
                {
                    new ProductAttribute(){Attribute="Display",Value="15.6",Product=products[0]},
                    new ProductAttribute(){Attribute="Processor",Value="Intel i7",Product=products[0]},
                    new ProductAttribute(){Attribute="Display",Value="15.6",Product=products[1]},
                    new ProductAttribute(){Attribute="Processor",Value="Intel i7",Product=products[1]},
                    new ProductAttribute(){Attribute="Display",Value="15.6",Product=products[2]},
                    new ProductAttribute(){Attribute="Processor",Value="Intel i7",Product=products[2]},
                    new ProductAttribute(){Attribute="Display",Value="15.6",Product=products[3]},
                    new ProductAttribute(){Attribute="Color",Value="Black",Product=products[3]},

                };
                context.Attributes.AddRange(attribute);
                context.SaveChanges();
            }
        }
    }
}
