﻿using Edura.WebUI.Entity;
using Edura.WebUI.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Edura.WebUI.Repository.Concrete.EntityFramework
{
    public class EfAttributeRepository : EfGenericRepository<ProductAttribute>, IAttributeRepository
    {
        public EfAttributeRepository(EduraContext context) : base(context)
        {

        }
        public EduraContext EduraContext
        {
            get { return context as EduraContext; }
        }
    }
}
