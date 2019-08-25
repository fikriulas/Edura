using Edura.WebUI.Repository.Abstract;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Components
{
    public class FeaturesProduct : ViewComponent
    {
        private IProductRepository repository;
        public FeaturesProduct(IProductRepository _repository)
        {
            repository = _repository;
        } 
        public IViewComponentResult Invoke()
        {
            return View(repository.GetAll().Where(i => i.IsFeatures && i.IsAppproved));
        }
    }
}
