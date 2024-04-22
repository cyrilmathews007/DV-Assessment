using Domain;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services
{
    public interface IProductService
    {
        Task<ProductDetailsResponse> GetProductAsync(string id);
        Task<ProductsResponse> GetProductsAsync();
    }
}
