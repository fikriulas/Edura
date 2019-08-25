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
using PagedList;

namespace Edura.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private IUnitOfWork unitOfWork;
        private EduraContext context;
        public AdminController(IUnitOfWork _unitOfWork, EduraContext _context)
        {
            unitOfWork = _unitOfWork;
            context = _context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CatalogList()
        {

            var model = new CatalogListModel()
            {
                Categories = unitOfWork.Categories.GetAll().ToList(),
                Products = unitOfWork.Products.GetAll().ToList(),

            };
            ViewBag.SuccessSave = TempData["SuccessSave"] ?? null;
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Category entity)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Categories.Add(entity);
                unitOfWork.SaveChanges();
                return Ok(entity); // succes çalışır. Badrequest de error çalışır.
            }
            return BadRequest();
        }
        [HttpGet]
        public IActionResult EditCategory(int id)
        {
            var entity = unitOfWork.Categories.GetAll()
                .Include(i => i.productCategories)
                .ThenInclude(i => i.Product)
                .Where(i => i.Id == id)
                .Select(i => new AdminEditCategoryModel()
                {
                    CategoryId = i.Id,
                    Name = i.Name,
                    Products = i.productCategories.Select(a => new AdminEditCategoryProduct()
                    {
                        ProductId = a.Product.Id,
                        ProductName = a.Product.Name,
                        Image = a.Product.Image,
                        IsAppproved = a.Product.IsAppproved,
                        IsFeatures = a.Product.IsFeatures,
                        IsHome = a.Product.IsHome,
                    }).ToList()
                }).FirstOrDefault();
            return View(entity);
        }
        [HttpPost]
        public IActionResult EditCategory(Category entity)
        {

            if (ModelState.IsValid)
            {
                unitOfWork.Categories.Edit(entity);
                unitOfWork.SaveChanges();
                TempData["SuccessSave"] = "Category Successfully Changed.";
                //redirecktoaction olunca viewbag doğrudan veri göndermiyor. 
                // temp data ile cataloglist action'a veri gönderiyoruz.
                return RedirectToAction("CatalogList");
            }
            else
            {
                return View("error");
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveFromCategory(int ProductId, int CategoryId)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Categories.RemoveFromCategory(ProductId, CategoryId);
                unitOfWork.SaveChanges();
                return Ok();
            }
            return BadRequest();
        }
        public IActionResult CategoryDelete(int id)
        {
            var category = unitOfWork.Categories.GetAll()
                .Where(i => i.Id == id)
                .FirstOrDefault();
            if (category != null)
            {
                unitOfWork.Categories.Delete(category);
                unitOfWork.SaveChanges();
                TempData["SuccessSave"] = "Category is delete Successfuly!";
            }
            else
            {
                //Category bulunamadı hatası
            }
            return RedirectToAction("CatalogList");
        }

    }
}



