using Edura.WebUI.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Edura.WebUI.Repository.Abstract
{
    public interface IProductRepository:IGenericRepository<Product>
    {
        /*
        Product Get(int id);
        IQueryable<Product> GetAll();
        IQueryable<Product> Find(Expression<Func<Product,bool>> predicate);
        void Add(Product entity);
        void Delete(Product entity);
        void Edit(Product entity);
        void Save();
        */
        // IGenericRepository'teki tüm methodlar tanımlandı. 
        // istenirse product'nesnesine özel methodlar tanımlanabilir.
        void AddProductsCategory(string[] categories,int productId);
        void UpdateProductsCategory(string[] categories, int productId);
        void AddProductAttribute(string[] attribute, string[] value, int productId);
        

    }
}
