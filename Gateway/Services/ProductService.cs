using EasyNetQ;
using Domain;
using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using Messages;

namespace Gateway.Services
{
    public class ProductService : IProductService
    {
        private readonly IBus _bus;

        public ProductService(IBus bus)
        {
            _bus = bus;
        }

        public Product GetProduct(string id)
        {
            var request = new ProductDetailsRequest { ProductId = id };
            var response = _bus.Rpc.Request<ProductDetailsRequest, ProductDetailsResponse>(request);
            return response.Product;
        }

        public List<Product> GetProducts()
        {
            var request = new ProductsRequest();
            var response = _bus.Rpc.Request<ProductsRequest, ProductsResponse>(request);
            return response.Products;
        }
    }
}
