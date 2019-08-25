using Edura.WebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Repository.Concrete.EntityFramework
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly EduraContext dbContext;
        public EfUnitOfWork(EduraContext _dbContext)
        {
            dbContext = _dbContext ?? throw new ArgumentNullException("DbContext can not be null!");
        }
        private IProductRepository _products;
        private ICategoryRepository _categories;
        private IOrderRepository _orders;
        private IAttributeRepository _attributes;
        private IProductOfCategoryRepository _productOfCategory;
        private IImageRepository _imageRepository;
        private IReviewRepository _reviewRepository;
        public IProductRepository Products
        {
            get
            {
                return _products ?? (_products = new EfProductRepository(dbContext));
            }
        }

        public ICategoryRepository Categories
        {
            get
            {
                return _categories ?? (_categories = new EfCategoryRepository(dbContext));
            }
        }
        public IOrderRepository Orders
        {
            get
            {
                return _orders ?? (_orders = new EfOrderRepository(dbContext));
            }
        }

        public IAttributeRepository Attributes
        {
            get
            {
                return _attributes ?? (_attributes = new EfAttributeRepository(dbContext));
            }
        }

        public IProductOfCategoryRepository ProductOfCategory
        {
            get
            {
                return _productOfCategory ?? (_productOfCategory = new EfProductOfCategoryRepository(dbContext));
            }
        }

        public IImageRepository Images
        {
            get
            {
                return _imageRepository ?? (_imageRepository = new EfImageRepository(dbContext));
            }
        }

        public IReviewRepository Reviews
        {
            get
            {
                return _reviewRepository ?? (_reviewRepository = new EfReviewRepository(dbContext));
            }
        }

        public int SaveChanges()
        {
            try
            {
                return dbContext.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Dispose()
        {
            dbContext.Dispose();
        }

        
    }
}
