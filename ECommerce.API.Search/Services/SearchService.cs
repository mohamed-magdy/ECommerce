using ECommerce.API.Search.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Search.Services
{
    public class SearchService : ISearchService
    {
        private readonly IOrdersService ordersService;
        private readonly IProductsService productsService;
        private readonly ICustomersService customersService;

        public SearchService(IOrdersService ordersService,IProductsService productsService,ICustomersService customersService)
        {
            this.ordersService = ordersService;
            this.productsService = productsService;
            this.customersService = customersService;
        }
        public async Task<(bool IsSuccess, dynamic SearchResults)> SearchAsync(int customerId)
        {
            var customerResult = await customersService.GetCustomerAsync(customerId);
            var ordersResult = await ordersService.GetOrderAsync(customerId);
            var productsResult = await productsService.GetProductsAsync();
            if(ordersResult.IsSuccess)
            {
                foreach(var order in ordersResult.Orders)
                {
                    foreach(var item in order.OrderItems)
                    {
                        item.ProductName = productsResult.IsSuccess? 
                                            productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId).Name:
                                            "Product information is not available";
                    }
                }
                var result = new
                {
                    Customer = customerResult.IsSuccess ? customerResult.Customer :
                               new { Name = "Customer information is not available" },
                    Orders = ordersResult.Orders
                };
                return (true, result);
            }
            return (false, null);
        }
    }
}
