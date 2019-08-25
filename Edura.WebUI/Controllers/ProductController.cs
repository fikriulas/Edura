using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edura.WebUI.Entity;
using Edura.WebUI.Models;
using Edura.WebUI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edura.WebUI.Controllers
{
    public class ProductController : Controller
    {
        public int PageSize = 2; // bir sayfada gözükecek ürün sayısı.
        private IUnitOfWork unitOfWork;
        public ProductController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Details(int id)
        {
            var product = unitOfWork.Products.GetAll() // getall hepsini getirmez. Daha IQuerable çünkü.
                .Where(i => i.Id == id)
                .Include(i => i.Images)
                .Include(i => i.Review)
                .Include(i => i.Attributes)
                .Include(i => i.productCategories)
                .ThenInclude(i => i.Category) // iki sonraki class'a gidiyoruz.
                .Select(i => new ProductDetailsModel()
                {
                    Product = i,
                    ProductImages = i.Images,
                    ProductAttributes = i.Attributes,
                    Reviews = i.Review,
                    Categories = i.productCategories.Select(a => a.Category).ToList(),
                }).FirstOrDefault();
            if (product != null)
            {

                return View(product);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult List(string category, int page = 1) // page varsayılan değer 1.
        {
            var product = unitOfWork.Products.GetAll();
            if (!string.IsNullOrEmpty(category))
            {
                product = product
                    .Include(i => i.productCategories)
                    .ThenInclude(i => i.Category)
                    .Where(i => i.productCategories.Any(a => a.Category.Name == category)); // any true yada false gönderir.

            }
            var count = product.Count();
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
        public IActionResult Review(Review entity, int productId)
        {
            if (ModelState.IsValid)
            {
                entity.ProductId = productId;
                entity.Date = DateTime.Now;
                unitOfWork.Reviews.Add(entity);
                unitOfWork.SaveChanges();
                return Ok();
            }
            //return RedirectToAction("Details",new { id=productId});
            return BadRequest();
        }
        public IActionResult ReviewDetails(int id)
        {
            var review = unitOfWork.Reviews.GetAll()
                .FirstOrDefault(i => i.Id == id);
            return View(review);
        }
        public IActionResult ReviewDelete(int id)
        {
            var review = unitOfWork.Reviews.GetAll()
                .FirstOrDefault(i => i.Id == id);
            unitOfWork.Reviews.Delete(review);
            unitOfWork.SaveChanges();
            return RedirectToAction("EditProduct", "Admin", new { id = review.ProductId });
        }
    }
}