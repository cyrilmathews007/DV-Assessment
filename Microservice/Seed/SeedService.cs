using Microservice.Entities;
using Microservice.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microservice.Seed
{
    public class SeedService : ISeedService
    {
        private readonly IProductRepository _productRepository;
        private readonly IPriceReductionRepository _reductionRepository;

        public SeedService(IProductRepository productRepository, IPriceReductionRepository reductionRepository)
        {
            _productRepository = productRepository;
            _reductionRepository = reductionRepository;
        }

        public async Task PopulateDataAsync()
        {
            if (await _productRepository.HasRecordsAsync())
            {
                return;
            }

            var random = new Random();

            var products = new List<Product> 
            {
                new Product
                {
                    EntryDate = DateTime.Now,
                    Name = "Product_" + random.Next(0, 10),
                    Price = random.Next(10, 1000),
                },
                new Product
                {
                    EntryDate = DateTime.Now,
                    Name = "Product_" + random.Next(0, 10),
                    Price = random.Next(10, 1000),
                },
                new Product
                {
                    EntryDate = DateTime.Now,
                    Name = "Product_" + random.Next(0, 10),
                    Price = random.Next(10, 1000),
                },
                new Product
                {
                    EntryDate = DateTime.Now,
                    Name = "Product_" + random.Next(0, 10),
                    Price = random.Next(10, 1000),
                },
                new Product
                {
                    EntryDate = DateTime.Now,
                    Name = "Product_" + random.Next(0, 10),
                    Price = random.Next(10, 1000),
                }
            };

            var reductions = new List<PriceReduction>
            {
                new PriceReduction
                {
                    DayOfWeek = 0,
                    Reduction = 0
                },
                new PriceReduction
                {
                    DayOfWeek = 1,
                    Reduction = 0
                },
                new PriceReduction
                {
                    DayOfWeek = 2,
                    Reduction = 0
                },
                new PriceReduction
                {
                    DayOfWeek = 3,
                    Reduction = 0
                },
                new PriceReduction
                {
                    DayOfWeek = 4,
                    Reduction = 0
                },
                new PriceReduction
                {
                    DayOfWeek = 5,
                    Reduction = 0.2
                },
                new PriceReduction
                {
                    DayOfWeek = 6,
                    Reduction = 0.5
                },
            };

            await _productRepository.AddProductsAsync(products);
            await _reductionRepository.AddPriceReductionsAsync(reductions);
        }
    }
}