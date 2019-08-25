using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edura.WebUI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Edura.WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IProductRepository repository;
        private IUnitOfWork unitOfWork;
        public HomeController(IUnitOfWork _unitOfWork,IProductRepository _repository)
        {
            repository = _repository;
            unitOfWork = _unitOfWork;
        }
        public IActionResult Index()
        {
            return View(unitOfWork.Products.GetAll().Where(i=>i.IsAppproved && i.IsHome));
        }
    }
}