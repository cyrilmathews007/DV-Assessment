using AutoFixture;
using Domain;
using Microservice.Entities;
using Microservice.Repository;
using Microservice.Services;
using MongoDB.Driver.Core.Misc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microservices.Test.Services.Test.ObjectIdCustomizations;

namespace Microservices.Test.Services.Test
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _productRepository;
        private readonly Mock<IPriceReductionService> _reductionService;
        private readonly Fixture _fixture;
        private readonly ProductService _productService;

        public ProductServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new ProductObjectIdCustomization());
            _productRepository = new Mock<IProductRepository>();
            _reductionService = new Mock<IPriceReductionService>();
            _productService = new ProductService(_productRepository.Object, _reductionService.Object);
        }

        [Fact]
        public async Task GetProductsAsync_ReturnsProducts()
        {
            var productEntitites = _fixture.CreateMany<Microservice.Entities.Product>(10).ToList();
            _productRepository.Setup(repo => repo.GetProductsAsync()).Returns(Task.FromResult(productEntitites));
            _reductionService.Setup(service => service.PerformPriceReductionAsync(It.IsAny<List<Domain.Product>>()));

            var productResponse = await _productService.GetProductsAsync();

            Assert.NotNull(productResponse?.Products);
            Assert.IsType<ProductsResponse>(productResponse);
            Assert.Equal(10, productResponse.Products.Count);
        }

        [Fact]
        public async Task GetProductsAsync_ReturnsEmptyProductList()
        {
            var productEntitites = _fixture.CreateMany<Microservice.Entities.Product>(0).ToList();
            _productRepository.Setup(repo => repo.GetProductsAsync()).Returns(Task.FromResult(productEntitites));
            _reductionService.Setup(service => service.PerformPriceReductionAsync(It.IsAny<List<Domain.Product>>()));

            var productResponse = await _productService.GetProductsAsync();

            Assert.NotNull(productResponse?.Products);
            Assert.Empty(productResponse.Products);
        }

        [Fact]
        public async Task GetProductAsync_WithValidProductId_ReturnsProduct()
        {
            var product = _fixture.Create<Microservice.Entities.Product>();
            _productRepository.Setup(repo => repo.GetProductAsync(It.IsAny<string>())).Returns(Task.FromResult(product));
            _reductionService.Setup(service => service.PerformPriceReductionAsync(It.IsAny<List<Domain.Product>>()));

            var productId = product._id.ToString();

            var productResponse = await _productService.GetProductAsync(productId);

            Assert.NotNull(productResponse?.Product);
            Assert.Equal(productId, productResponse.Product.Id);
        }

        [Fact]
        public async Task GetProductAsync_WithInValidProductId_ReturnsNullProduct()
        {
            _productRepository.Setup(repo => repo.GetProductAsync(It.IsAny<string>())).Returns(Task.FromResult((Microservice.Entities.Product)null));
            _reductionService.Setup(service => service.PerformPriceReductionAsync(It.IsAny<Domain.Product>()));

            var productResponse = await _productService.GetProductAsync("");

            Assert.NotNull(productResponse);
            Assert.Null(productResponse.Product);
        }
    }
}
