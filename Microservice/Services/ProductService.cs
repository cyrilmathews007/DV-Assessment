﻿using Domain;
using Messages;
using Microservice.Entities;
using Microservice.Repository;
using SharpCompress.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPriceReductionService _reductionService;

        public ProductService(IProductRepository productRepository, IPriceReductionService reductionService)
        {
            _productRepository = productRepository;
            _reductionService = reductionService;
        }

        public async Task<ProductsResponse> GetProductsAsync()
        {
            var productEntities = await _productRepository.GetProductsAsync();
            var products = productEntities.Select(entity => MapProduct(entity)).ToList();
            await _reductionService.PerformPriceReductionAsync(products);
            return new ProductsResponse { Products = products };
        }

        public async Task<ProductDetailsResponse> GetProductAsync(string id)
        {
            var response = new ProductDetailsResponse();
            var productEntity = await _productRepository.GetProductAsync(id);

            if (productEntity == null)
            {
                return response;
            }
            
            var product = MapProduct(productEntity);
            await _reductionService.PerformPriceReductionAsync(product);
            return new ProductDetailsResponse { Product = product };
        }

        private static Domain.Product MapProduct(Entities.Product productEntity)
        {
            return new Domain.Product
            {
                Name = productEntity.Name,
                EntryDate = productEntity.EntryDate,
                Id = productEntity._id.ToString(),
                PriceWithReduction = productEntity.Price
            };
        }
    }
}
