using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edura.WebUI.Entity;
using Edura.WebUI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Edura.WebUI.Controllers
{
    public class ReviewController : Controller
    {
        private IUnitOfWork unitOfWork;
        public ReviewController(IUnitOfWork _unitOfWork)
        {
            unitOfWork = _unitOfWork;
        }
        public IActionResult Review(Review entity,int productId)
        {
            if(ModelState.IsValid)
            {
                entity.ProductId = productId;
                entity.Date = DateTime.Now;                
                unitOfWork.Reviews.Add(entity);
                unitOfWork.SaveChanges();
            }
            return RedirectToAction("Details","Product",productId);
        }
        
    }
}