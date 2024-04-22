using Microservice.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Repository
{
    public interface IProductRepository
    {
        Task AddProductsAsync(List<Product> products);
        Task<Product> GetProductAsync(string id);
        Task<List<Product>> GetProductsAsync();
        Task<bool> HasRecordsAsync();
    }
}
