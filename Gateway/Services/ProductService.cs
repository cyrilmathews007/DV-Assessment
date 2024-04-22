using EasyNetQ;
using Domain;
using Microsoft.AspNetCore.Hosting.Server;
using System;
using System.Collections.Generic;
using Messages;
using System.Threading.Tasks;

namespace Gateway.Services
{
    public class ProductService : IProductService
    {
        private readonly IBus _bus;

        public ProductService(IBus bus)
        {
            _bus = bus;
        }

        public async Task<Product> GetProductAsync(string id)
        {
            var request = new ProductDetailsRequest { ProductId = id };
            var response = await _bus.Rpc.RequestAsync<ProductDetailsRequest, ProductDetailsResponse>(request);
            return response.Product;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var request = new ProductsRequest();
            var response = await _bus.Rpc.RequestAsync<ProductsRequest, ProductsResponse>(request);
            return response.Products;
        }
    }
}
