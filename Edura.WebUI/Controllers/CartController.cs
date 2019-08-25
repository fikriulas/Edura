using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edura.WebUI.Entity;
using Edura.WebUI.Infrastructure;
using Edura.WebUI.Models;
using Edura.WebUI.Repository.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Edura.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        private IUnitOfWork unitOfWork;
        public CartController(IProductRepository _repository, IUnitOfWork _unitOfWork)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
        }

        public IActionResult Index()
        {
            return View(GetCart());
        }
        public IActionResult AddToCard(int productId, int quantity = 1) //varsayılan değer.
        {
            var product = repository.Get(productId);
            if (product != null)
            {
                var cart = GetCart();
                cart.AddProduct(product, quantity);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }
        public IActionResult RemoveFromCart(int ProductId)
        {
            var product = repository.Get(ProductId);
            if (product != null)
            {
                var cart = GetCart();
                cart.RemoveProduct(product);
                SaveCart(cart);
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult Checkout()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public IActionResult Checkout(OrderDetails model)
        {
            var cart = GetCart();
            if (cart.Products.Count == 0)
            {
                ModelState.AddModelError("DoesntCart", "You dont have any product.");
            }
            if (ModelState.IsValid)
            {
                SaveOrder(cart, model);
                cart.ClearAll();
                SaveCart(cart);
                return RedirectToAction("Complate",1); // tamamlandı view oluşur.
            }
            return View(model);
        }
        private void SaveOrder(Cart cart, OrderDetails details)
        {
            var order = new Order();
            order.orderNumber = "A" + (new Random()).Next(11111, 99999).ToString();
            order.Total = cart.TotalPrice();
            order.OrderDate = DateTime.Now;
            order.OrderState = EnumOrderState.Waiting;
            order.Username = User.Identity.Name;
            order.AdresBasligi = details.AdresBasligi;
            order.Adres = details.Adres;
            order.Sehir = details.Sehir;
            order.Telefon = details.Telefon;
            order.Semt = details.Semt;
            foreach (var product in cart.Products)
            {
                var orderline = new OrderLine();
                orderline.Quantity = product.Quantity;
                orderline.Price = product.Product.Price;
                orderline.ProductId = product.Product.Id;
                order.OrderLines.Add(orderline);
            }
            unitOfWork.Orders.Add(order);
            unitOfWork.Orders.Save();

        }
        private void SaveCart(Cart cart)
        {
            HttpContext.Session.SetJson("Cart", cart);
        }

        private Cart GetCart()
        {
            return HttpContext.Session.GetJson<Cart>("Cart") ?? new Cart(); // boş ise cart gönder.
        }
        public IActionResult Completed(int id)
        {
            var complate = unitOfWork.Orders.GetAll()
                .Where(i => i.Id == id) 
                .Include(i => i.OrderLines)
                .ThenInclude(i => i.Product)
                .Select(i => new OrderComplateModel()
                {
                    Order = i,
                    OrderLine = i.OrderLines,
                    Product = i.OrderLines.Select(b => b.Product).FirstOrDefault(),
                }).FirstOrDefault();
                
            return View(complate);
        }
    }
}