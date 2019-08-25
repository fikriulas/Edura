using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Edura.WebUI.Entity;
using Edura.WebUI.Models;
using Edura.WebUI.Repository.Abstract;
using Edura.WebUI.Repository.Concrete.EntityFramework;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Edura.WebUI.Controllers.Admin
{

    public class AdminProductController : Controller
    {
        public int PageSize = 3; // bir sayfada gözükecek ürün sayısı.
        private IUnitOfWork unitOfWork;
        private EduraContext context;
        public AdminProductController(IUnitOfWork _unitOfWork, EduraContext _context)
        {

            unitOfWork = _unitOfWork;
            context = _context;
        }
        public IActionResult Index(int page = 1)
        {
            /*
            var products = unitOfWork.Products.GetAll().ToList();         
            ViewBag.SuccessSave = TempData["SuccessSave"] ?? null;            
            return View(products);
            */
            
            var product = unitOfWork.Products.GetAll();            
            var count = product.Count();
            // aaaa
            product = product.Skip((page - 1) * PageSize).Take(PageSize);
            return View(
                    new ProductListModel()
                    {
                        Products = product,
                        PagingInfo = new PagingInfo()
                        {
                            CurrentPage = page,
                            ItemsPerPage = PageSize,
                            TotalItems = count
                        }
                    }
                );
                
        }
        [HttpGet]
        public IActionResult AddProduct()
        {
            var Categories = new List<SelectListItem>();
            foreach (var item in unitOfWork.Categories.GetAll())
            {
                Categories.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            ViewBag.Categories = Categories;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddProduct(Product entity, IFormFile mainFile, IFormFile[] file, string[] category, string[] attribute, string[] value)
        {
            if (ModelState.IsValid)
            {
                var path = "";
                if (mainFile != null)
                {
                    path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\edura\\images\\products\\", mainFile.FileName);
                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await mainFile.CopyToAsync(stream);
                        entity.Image = mainFile.FileName;
                    }
                }
                entity.DateAdded = DateTime.Now;
                unitOfWork.Products.Add(entity);
                unitOfWork.SaveChanges();
                if (file.Length != 0)
                {
                    List<Image> productOfImage = new List<Image>();
                    for (int i = 0; i < file.Length; i++)
                    {
                        path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\edura\\images\\products\\", file[i].FileName);
                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file[i].CopyToAsync(stream);
                            var images = new Image()
                            {
                                Product = entity,
                                Name = file[i].FileName,
                            };
                            productOfImage.Add(images);
                        }
                    }
                    unitOfWork.Images.AddRange(productOfImage);
                }
                /*              
                entity.Attributes = new List<ProductAttribute>
                {                    
                    new ProductAttribute
                    {
                        Product = entity,
                        Attribute = attribute[0],
                        Value = value[0],
                    },
                    
                    new ProductAttribute
                    {
                        Product = entity,
                        Attribute = attribute[0],
                        Value = value[0],
                    },                    
                };
                */
                // add attribute
                List<ProductAttribute> productOfAttributes = new List<ProductAttribute>();
                for (int i = 0; i < attribute.Count(); i++)
                {
                    var attributes = new ProductAttribute()
                    {
                        Product = entity,
                        Attribute = attribute[i],
                        Value = value[i],
                    };
                    productOfAttributes.Add(attributes);
                }
                unitOfWork.Attributes.AddRange(productOfAttributes);
                // add category
                List<ProductCategory> productOfCategories = new List<ProductCategory>();
                for (int i = 0; i < category.Count(); i++)
                {
                    var categories = new ProductCategory()
                    {
                        Product = entity,
                        CategoryId = Convert.ToInt32(category[i]),
                    };
                    productOfCategories.Add(categories);
                }
                unitOfWork.ProductOfCategory.AddRange(productOfCategories);
                unitOfWork.SaveChanges();




                return RedirectToAction("CatalogList");

                //
                /*
                 * 
                entity.Attributes.Clear();
                for (int i = 0; i < attribute.Count(); i++)
                {
                    entity.Attributes.Add(new ProductAttribute()
                    {
                        ProductId = entity.Id,
                        Attribute = attribute[i],
                        Value = value[i],
                    });                    
                }
                for (int i = 0; i < category.Length; i++)
                {
                    entity.productCategories.Add(new ProductCategory()
                    {
                        CategoryId = Convert.ToInt32(category[i]),
                        ProductId = entity.Id,
                    });
                }                
                for (int i = 0; i < attribute.Count(); i++)
                {
                    var attributes = new ProductAttribute()
                    {
                        Product = entity,
                        Attribute = attribute[i],
                        Value = value[i],
                    };
                    unitOfWork.Attributes.Add(attributes);
                    unitOfWork.SaveChanges();
                }
                */
            }
            return View(entity);
        }
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            var product = unitOfWork.Products.GetAll()
                .Where(i => i.Id == id)
                .Include(i => i.Images)
                .Include(i => i.productCategories)
                .ThenInclude(i => i.Category)
                .Select(i => new EditProductModel()
                {
                    Id = i.Id,
                    Name = i.Name,
                    Price = i.Price,
                    Description = i.Description,
                    HtmlContent = i.HtmlContent,
                    Image = i.Image,
                    IsAppproved = i.IsAppproved,
                    IsFeatures = i.IsFeatures,
                    IsHome = i.IsHome,
                    Reviews = i.Review.Select(d => new AdminEditReview()
                    {
                        Id = d.Id,
                        Name = d.Name,
                        Date = d.Date,
                        ProductId = d.ProductId
                    }).ToList(),
                    Images = i.Images.Select(c => new AdminEditImageGallery()
                    {
                        Id = c.Id,
                        Name = c.Name
                    }).ToList(),
                    Attributes = i.Attributes.Select(b => new AdminEditProductAttribute()
                    {
                        Id = b.Id,
                        Attribute = b.Attribute,
                        Value = b.Value
                    }).ToList(),
                    Category = i.productCategories.Select(a => new AdminEditProductCategory()
                    {
                        CategoryId = a.Category.Id,
                        Name = a.Category.Name
                    }).ToList()

                }).FirstOrDefault();
            var Categories = new List<SelectListItem>();
            foreach (var item in unitOfWork.Categories.GetAll())
            {
                Categories.Add(new SelectListItem
                {
                    Text = item.Name,
                    Value = item.Id.ToString()
                });
            }
            ViewBag.Categories = Categories;
            return View(product);
        }
        public async Task<IActionResult> EditProduct(Product entity, IFormFile mainFile, IFormFile[] file, string[] category, string[] attribute, string[] value, string[] attid)
        {
            if (ModelState.IsValid)
            {
                if (mainFile != null)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\edura\\images\\products\\", entity.Image);
                    System.IO.File.Delete(path);
                    var pathdelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\edura\\images\\products\\", mainFile.FileName);
                    using (var stream = new FileStream(pathdelete, FileMode.Create))
                    {
                        await mainFile.CopyToAsync(stream);
                        entity.Image = mainFile.FileName;
                    }
                }
                Product AttributeOfProduct = unitOfWork.Products.GetAll()
                    .Include(p => p.Attributes)
                    .Include(p => p.productCategories)
                    .Include(p => p.Images)
                    .FirstOrDefault(p => p.Id == entity.Id);
                // attribute guncellenmesi için
                AttributeOfProduct.Attributes.Clear();
                for (int i = 0; i < attribute.Count(); i++)
                {
                    AttributeOfProduct.Attributes.Add(new ProductAttribute()
                    {
                        Product = entity,
                        Attribute = attribute[i],
                        Value = value[i],
                    });
                }
                // category güncellemesi için
                AttributeOfProduct.productCategories.Clear();
                for (int i = 0; i < category.Length; i++)
                {
                    AttributeOfProduct.productCategories.Add(new ProductCategory()
                    {
                        CategoryId = Convert.ToInt32(category[i]),
                        ProductId = entity.Id,
                    });
                }
                // image galery güncellemesi için.
                var pathUpdate = "";
                var pathDelete = "";


                if (file.Length != 0)
                {
                    var imageList = unitOfWork.Products.GetAll()
                        .Include(p => p.Images)
                        .FirstOrDefault(p => p.Id == entity.Id);
                    foreach (var item in imageList.Images)
                    {
                        pathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\edura\\images\\products\\", item.Name);
                        System.IO.File.Delete(pathDelete);
                    }
                    AttributeOfProduct.Images.Clear();
                    for (int j = 0; j < file.Length; j++)
                    {
                        pathUpdate = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\edura\\images\\products\\", file[j].FileName);
                        using (var stream = new FileStream(pathUpdate, FileMode.Create))
                        {
                            await file[j].CopyToAsync(stream);
                            AttributeOfProduct.Images.Add(new Image()
                            {
                                Product = entity,
                                Name = file[j].FileName,
                            });
                        }
                    }
                }
                AttributeOfProduct.Name = entity.Name;
                AttributeOfProduct.Price = entity.Price;
                AttributeOfProduct.Description = entity.Description;
                AttributeOfProduct.HtmlContent = entity.HtmlContent;
                AttributeOfProduct.IsAppproved = entity.IsAppproved;
                AttributeOfProduct.IsFeatures = entity.IsFeatures;
                AttributeOfProduct.IsHome = entity.IsHome;
                AttributeOfProduct.Image = entity.Image;
                unitOfWork.Products.Edit(AttributeOfProduct);
                unitOfWork.SaveChanges();

                /*
                if (category.Count() != 0)
                {
                    //kullanıma gerek kalmadı.
                    unitOfWork.Products.UpdateProductsCategory(category, entity.Id);
                }
                */
                return RedirectToAction("CatalogList");
            }
            return View();
        }
        public IActionResult ProductDelete(int productId)
        {
            var product = unitOfWork.Products.GetAll()
                .Include(i => i.Attributes)
                .Include(i => i.Images)
                .Include(i => i.productCategories)
                .Where(i => i.Id == productId)
                .FirstOrDefault();
            if (product != null)
            {
                var pathDelete = "";
                foreach (var item in product.Images)
                {   // image gallerydeki fotoğrafraları siler.
                    pathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\edura\\images\\products\\", item.Name);
                    System.IO.File.Delete(pathDelete);
                }
                pathDelete = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\edura\\images\\products\\", product.Image);
                System.IO.File.Delete(pathDelete); // ürünün fotoğrafını siler.


                unitOfWork.Products.Delete(product);
                unitOfWork.SaveChanges();
                TempData["SuccessSave"] = "Product is delete Successfuly!";

            }
            else
            {
                //ürün bulunamadı hatası
            }
            return RedirectToAction("CatalogList");
        }
    }
}