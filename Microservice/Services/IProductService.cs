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
        ProductDetailsResponse GetProduct(string id);
        ProductsResponse GetProducts();
    }
}
