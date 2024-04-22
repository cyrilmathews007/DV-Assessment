using AutoFixture;
using Microservice.Repository;
using Microservice.Services;
using Moq;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task PerformPriceReduction_ListOfProducts_ReductionIsApplied()
        {
            var products = _fixture.CreateMany<Product>(10).ToList();
            var productActualPrices = products.Select(product => new { Id = product.Id, Price = product.PriceWithReduction }).ToList();
            var reduction = 0.5;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeekAsync(It.IsAny<int>())).Returns(Task.FromResult(reduction));

            await _reductionService.PerformPriceReductionAsync(products);

            Assert.NotNull(products);
            Assert.NotEmpty(products);

            foreach (var product in products)
            {
                var actualPrice = productActualPrices.First(actual => actual.Id == product.Id).Price;
                Assert.Equal(actualPrice - (actualPrice * reduction), product.PriceWithReduction);
            }
        }

        [Fact]
        public async Task PerformPriceReduction_ListOfProducts_ZeroReduction()
        {
            var products = _fixture.CreateMany<Product>(10).ToList();
            var productActualPrices = products.Select(product => new { Id = product.Id, Price = product.PriceWithReduction }).ToList();
            var reduction = 0.0;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeekAsync(It.IsAny<int>())).Returns(Task.FromResult(reduction));

            await _reductionService.PerformPriceReductionAsync(products);

            Assert.NotNull(products);
            Assert.NotEmpty(products);

            foreach (var product in products)
            {
                var actualPrice = productActualPrices.First(actual => actual.Id == product.Id).Price;
                Assert.Equal(actualPrice, product.PriceWithReduction);
            }
        }

        [Fact]
        public async Task PerformPriceReduction_EmptyList_NoFailure()
        {
            var products = _fixture.CreateMany<Product>(0).ToList();
            var reduction = 0.5;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeekAsync(It.IsAny<int>())).Returns(Task.FromResult(reduction));

            await _reductionService.PerformPriceReductionAsync(products);

            Assert.Empty(products);
        }

        [Fact]
        public async Task PerformPriceReduction_SingleProduct_ReductionIsApplied()
        {
            var product = _fixture.Create<Product>();
            var productActualPrice = product.PriceWithReduction;
            var reduction = 0.5;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeekAsync(It.IsAny<int>())).Returns(Task.FromResult(reduction));

            await _reductionService.PerformPriceReductionAsync(product);

            Assert.NotNull(product);

            Assert.Equal(productActualPrice - (productActualPrice * reduction), product.PriceWithReduction);
        }

        [Fact]
        public async Task PerformPriceReduction_SingleProduct_ZeroReduction()
        {
            var product = _fixture.Create<Product>();
            var productActualPrice = product.PriceWithReduction;
            var reduction = 0.0;

            _reductionRepository.Setup(repo => repo.GetPriceReductionByDayOfWeekAsync(It.IsAny<int>())).Returns(Task.FromResult(reduction));

            await _reductionService.PerformPriceReductionAsync(product);

            Assert.NotNull(product);

            Assert.Equal(productActualPrice, product.PriceWithReduction);
        }

    }
}
