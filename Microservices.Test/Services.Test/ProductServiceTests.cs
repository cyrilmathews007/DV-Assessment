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
        public void GetAuctions_ReturnsProducts()
        {
            var productEntitites = _fixture.CreateMany<Microservice.Entities.Product>(10).ToList();
            _productRepository.Setup(repo => repo.GetProducts()).Returns(productEntitites);
            _reductionService.Setup(service => service.PerformPriceReduction(It.IsAny<List<Domain.Product>>()));

            var productResponse = _productService.GetProducts();

            Assert.NotNull(productResponse?.Products);
            Assert.IsType<ProductsResponse>(productResponse);
            Assert.Equal(10, productResponse.Products.Count);
        }

        [Fact]
        public void GetAuctions_ReturnsEmptyProductList()
        {
            var productEntitites = _fixture.CreateMany<Microservice.Entities.Product>(0).ToList();
            _productRepository.Setup(repo => repo.GetProducts()).Returns(productEntitites);
            _reductionService.Setup(service => service.PerformPriceReduction(It.IsAny<List<Domain.Product>>()));

            var productResponse = _productService.GetProducts();

            Assert.NotNull(productResponse?.Products);
            Assert.Empty(productResponse.Products);
        }

        [Fact]
        public void GetAuction_WithValidProductId_ReturnsProduct()
        {
            var product = _fixture.Create<Microservice.Entities.Product>();
            _productRepository.Setup(repo => repo.GetProduct(It.IsAny<string>())).Returns(product);
            _reductionService.Setup(service => service.PerformPriceReduction(It.IsAny<List<Domain.Product>>()));

            var productId = product._id.ToString();

            var productResponse = _productService.GetProduct(productId);

            Assert.NotNull(productResponse?.Product);
            Assert.Equal(productId, productResponse.Product.Id);
        }

        [Fact]
        public void GetAuction_WithInValidProductId_ReturnsNullProduct()
        {
            _productRepository.Setup(repo => repo.GetProduct(It.IsAny<string>())).Returns((Microservice.Entities.Product) null);
            _reductionService.Setup(service => service.PerformPriceReduction(It.IsAny<Domain.Product>()));

            var productResponse = _productService.GetProduct("");

            Assert.NotNull(productResponse);
            Assert.Null(productResponse.Product);
        }
    }
}
