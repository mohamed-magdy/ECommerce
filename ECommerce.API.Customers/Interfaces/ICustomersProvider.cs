using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.API.Customers.Interfaces
{
    public interface ICustomersProvider
    {

        Task<(bool IsSuccess, IEnumerable<Models.Customer> customers, string errorMessage)> GetCustomersAsync();
        Task<(bool IsSuccess, Models.Customer customer, string errorMessage)> GetCustomerAsync(int id);
    }
}
