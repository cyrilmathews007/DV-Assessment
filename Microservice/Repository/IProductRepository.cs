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
        void AddProducts(List<Product> products);
        Product GetProduct(string id);
        List<Product> GetProducts();
        bool HasRecords();
    }
}
