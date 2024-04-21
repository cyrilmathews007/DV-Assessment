using Domain;
using System.Collections.Generic;

namespace Gateway.Services
{
    public interface IProductService
    {
        List<Product> GetProducts();
        Product GetProduct(string id);
    }
}
