using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public interface IProductService
    {
        Task<List<Product>> GetProductsAsync();
        Task<Product> GetProductAsync(string id);
    }
}
