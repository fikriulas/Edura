using Edura.WebUI.Entity;
using Edura.WebUI.Models;
using Edura.WebUI.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edura.WebUI.Repository.Concrete.EntityFramework
{
    public class EfCategoryRepository : EfGenericRepository<Category>, ICategoryRepository    //ICategoryRepository
    {
        public EfCategoryRepository(EduraContext context) : base(context)
        {

        }
        public EduraContext EduraContext
        {
            get { return context as EduraContext; }
        }
        public Category GetByName(string Name)
        {
            return EduraContext.Categories.Where(i => i.Name == Name).FirstOrDefault();
        }

        public IEnumerable<CategoryModel> GetProductCount()
        {
            return GetAll().Select(i => new CategoryModel()
            {
                Id = i.Id,
                Name = i.Name,
                Count = i.productCategories.Count()
            });
        }

        public void RemoveFromCategory(int ProductId, int CategoryId)
        {
            var cmd = $"delete from ProductCategory where ProductId= {ProductId} and CategoryId={CategoryId}";
            context.Database.ExecuteSqlCommand(cmd);
        }
    }
}
