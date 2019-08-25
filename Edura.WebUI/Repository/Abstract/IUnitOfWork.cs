using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Repository.Abstract
{
    public interface IUnitOfWork: IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IAttributeRepository Attributes { get; }
        IProductOfCategoryRepository ProductOfCategory { get; }
        IImageRepository Images { get; }
        IReviewRepository Reviews { get; }
        int SaveChanges();
    }
}
