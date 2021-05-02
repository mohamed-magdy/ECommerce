﻿using ECommerce.API.Products.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Products.Interfaces
{
    public interface IProductsProvider
    {
        Task<(bool IsSucess, IEnumerable<Product> products, string errorMessage)> GetProductsAsync();
        Task<(bool IsSucess, Product product, string errorMessage)> GetProductAsync(int id);
    }
}
