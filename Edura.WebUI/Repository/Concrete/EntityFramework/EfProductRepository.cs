using Edura.WebUI.Entity;
using Edura.WebUI.Repository.Abstract;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edura.WebUI.Repository.Concrete.EntityFramework
{
    public class EfProductRepository : EfGenericRepository<Product>, IProductRepository
    {
        public EfProductRepository(EduraContext context) : base(context)
        {

        }

        public EduraContext EduraContext
        {
            get { return context as EduraContext; } // özel methodlar için contex'e ihtiyaç var.
        }
        
        public void AddProductsCategory(string[] categories,int productId)
        {
            
            var cmd = "";            
            foreach (var item in categories)
            {
                cmd = $"INSERT INTO ProductCategory (ProductId,CategoryId) VALUES({productId},{Convert.ToInt32(item)})";
                context.Database.ExecuteSqlCommand(cmd);
            }
            
        }

        public void UpdateProductsCategory(string[] categories, int productId)
        {
            var cmd = $"delete from ProductCategory Where ProductId={productId}";
            context.Database.ExecuteSqlCommand(cmd);
            AddProductsCategory(categories, productId);
        }
        public void AddProductAttribute(string[] attribute,string[] value,int productId)
        {
            var entity = new ProductAttribute();
            for (int i = 0; i < attribute.Count(); i++)
            {
                entity.Attribute = attribute[i];
                entity.Value = value[i];
                entity.ProductId = productId;
                context.Set<ProductAttribute>().Add(entity);
                context.SaveChanges();
            }
            
        }
        
        /*
private EduraContext context;
public EfProductRepository(EduraContext _context)
{
context = _context;
}

public void Add(Product entity)
{
context.Products.Add(entity); 
}

public void Delete(Product entity)
{
context.Products.Remove(entity);
}

public void Edit(Product entity)
{
context.Entry<Product>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
}

public IQueryable<Product> Find(Expression<Func<Product, bool>> predicate)
{
return context.Products.Where(predicate);
}

public Product Get(int id)
{
return context.Products.FirstOrDefault(i => i.Id == id);
}

public IQueryable<Product> GetAll()
{
return context.Products;
}

public void Save()
{
context.SaveChanges();
}
*/
    }
}
