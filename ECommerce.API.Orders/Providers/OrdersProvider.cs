using AutoMapper;
using ECommerce.API.Orders.Db;
using ECommerce.API.Orders.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Orders.Providers
{
    public class OrdersProvider : IOrdersProvider
    {
        private readonly OrdersDbContext ordersDbContext;
        private readonly ILogger<OrdersProvider> logger;
        private readonly IMapper mapper;

        public OrdersProvider(OrdersDbContext ordersDbContext, ILogger<OrdersProvider> logger, IMapper mapper)
        {
            this.ordersDbContext = ordersDbContext;
            this.logger = logger;
            this.mapper = mapper;

            SeedData();
        }

        private void SeedData()
        {
            if (!ordersDbContext.Orders.Any())
            {
                ordersDbContext.Orders.Add(new Db.Order()
                {
                    Id = 3,
                    CustomerId = 2,
                    OrderDate = new DateTime(2019, 12, 11),
                    Total = 100,
                    OrderItems = new List<OrderItem> (){
                 new OrderItem(){Id = 11,ProductId=1,Quantity=10,UnitPrice=10},
                 new OrderItem(){Id =12,ProductId=2,Quantity =10,UnitPrice=10},
                 new OrderItem(){Id=13,ProductId=3,Quantity=1,UnitPrice=100}
                }
                });
                ordersDbContext.Orders.Add(new Db.Order()
                {
                    Id = 1,
                    CustomerId = 1,
                    OrderDate = new DateTime(2019, 12, 11),
                    Total = 100,
                    OrderItems = new List<OrderItem>(){
                 new OrderItem(){Id = 1,ProductId=1,Quantity=10,UnitPrice=10},
                 new OrderItem(){Id =2,ProductId=2,Quantity =10,UnitPrice=10},
                 new OrderItem(){Id=3,ProductId=3,Quantity=10,UnitPrice=10},
                 new OrderItem(){Id=4,ProductId=2,Quantity=10,UnitPrice=10},
                 new OrderItem(){Id=5,ProductId=3,Quantity=1,UnitPrice=100},
                }
                });
                ordersDbContext.Orders.Add(new Db.Order()
                {
                    Id = 2,
                    CustomerId = 1,
                    OrderDate = new DateTime(2019, 12, 10),
                    Total = 100,
                    OrderItems = new List<OrderItem>(){
                 new OrderItem(){Id = 6,ProductId=1,Quantity=10,UnitPrice=10},
                 new OrderItem(){Id =7,ProductId=2,Quantity =10,UnitPrice=10},
                 new OrderItem(){Id=8,ProductId=3,Quantity=10,UnitPrice=10},
                 new OrderItem(){Id=9,ProductId=2,Quantity=10,UnitPrice=10},
                 new OrderItem(){Id=10,ProductId=3,Quantity=1,UnitPrice=100},
                }
                });

                ordersDbContext.SaveChanges();
            }
        }

        public async Task<(bool IsSuccess, IEnumerable<Models.Order> orders, string errorMessage)> GetOrdersAsync(int customerId)
        {
            try
            {
                var orders = await ordersDbContext.Orders.Where(o => o.CustomerId == customerId).ToListAsync();
                if (orders != null && orders.Any())
                {
                    var result = mapper.Map<IEnumerable<Db.Order>, IEnumerable<Models.Order>>(orders);
                    return (true, result, null);
                }
                return (false, null, "Not Found");
            }
            catch (Exception ex)
            {
                logger.LogError(ex.ToString());
                return (false, null, ex.Message);
            }
        }
    }
}
