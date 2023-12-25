﻿using ProniaOnion202.Application.Abstractions.Repositories;
using ProniaOnion202.Domain.Entities;
using ProniaOnion202.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProniaOnion202.Persistence.Implementations.Repositories
{
    internal class ProductRepository:Repository<Product>,IProductRepository
    {
        public ProductRepository(AppDbContext context):base(context) { }

    }
}
