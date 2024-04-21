using AutoFixture;
using Microservice.Repository;
using Microservice.Services;
using Moq;
using System.Linq;
using static Microservices.Test.Services.Test.ObjectIdCustomizations;
using Product = Domain.Product;

namespace Microservices.Test.Services.Test
{
    public class PriceReductionServiceTests
    {
        private readonly Mock<IPriceReductionRepository> _reductionRepository;
        private readonly Fixture _fixture;
        private readonly PriceReductionService _reductionService;

        public PriceReductionServiceTests()
        {
            _fixture = new Fixture();
            _fixture.Customize(new PriceReductionObjectIdCustomization());
            _reductionRepository = new Mock<IPriceReductionRepository>();
            _reductionService = new PriceReductionService(_reductionRepository.Object);
        }

        [Fact]
        public void PerformPriceReduction_ListOfProducts_ReductionIsApplied()
        {
            var products = _fixture.CreateMany<Product>(10).ToList();
            var productActualPrices = products.Select(product => new { Id = product.Id, Price = product.PriceWithReduction }).ToList();
            var reduction = 0.5;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeek(It.IsAny<int>())).Returns(reduction);

            _reductionService.PerformPriceReduction(products);

            Assert.NotNull(products);
            Assert.NotEmpty(products);

            foreach (var product in products)
            {
                var actualPrice = productActualPrices.First(actual => actual.Id == product.Id).Price;
                Assert.Equal(actualPrice - (actualPrice * reduction), product.PriceWithReduction);
            }
        }

        [Fact]
        public void PerformPriceReduction_ListOfProducts_ZeroReduction()
        {
            var products = _fixture.CreateMany<Product>(10).ToList();
            var productActualPrices = products.Select(product => new { Id = product.Id, Price = product.PriceWithReduction }).ToList();
            var reduction = 0;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeek(It.IsAny<int>())).Returns(reduction);

            _reductionService.PerformPriceReduction(products);

            Assert.NotNull(products);
            Assert.NotEmpty(products);

            foreach (var product in products)
            {
                var actualPrice = productActualPrices.First(actual => actual.Id == product.Id).Price;
                Assert.Equal(actualPrice, product.PriceWithReduction);
            }
        }

        [Fact]
        public void PerformPriceReduction_EmptyList_NoFailure()
        {
            var products = _fixture.CreateMany<Product>(0).ToList();
            var reduction = 0.5;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeek(It.IsAny<int>())).Returns(reduction);

            _reductionService.PerformPriceReduction(products);

            Assert.Empty(products);
        }

        [Fact]
        public void PerformPriceReduction_SingleProduct_ReductionIsApplied()
        {
            var product = _fixture.Create<Product>();
            var productActualPrice = product.PriceWithReduction;
            var reduction = 0.5;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeek(It.IsAny<int>())).Returns(reduction);

            _reductionService.PerformPriceReduction(product);

            Assert.NotNull(product);

            Assert.Equal(productActualPrice - (productActualPrice * reduction), product.PriceWithReduction);
        }

        [Fact]
        public void PerformPriceReduction_SingleProduct_ZeroReduction()
        {
            var product = _fixture.Create<Product>();
            var productActualPrice = product.PriceWithReduction;
            var reduction = 0;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeek(It.IsAny<int>())).Returns(reduction);

            _reductionService.PerformPriceReduction(product);

            Assert.NotNull(product);

            Assert.Equal(productActualPrice, product.PriceWithReduction);
        }

    }
}
