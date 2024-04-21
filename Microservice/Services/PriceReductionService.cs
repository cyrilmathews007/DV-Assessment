using Domain;
using Microservice.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services
{
    public class PriceReductionService : IPriceReductionService
    {
        private readonly IPriceReductionRepository _priceReductionRepository;

        public PriceReductionService(IPriceReductionRepository priceReductionRepository)
        {
            _priceReductionRepository = priceReductionRepository;
        }

        public void PerformPriceReduction(List<Product> products)
        {
            var reduction = _priceReductionRepository.GetPriceReductionByDayOfWeek((int)DateTime.Now.DayOfWeek);

            if (reduction > 0)
            {
                products.ForEach(product => ReducePrice(product, reduction));
            }
        }

        public void PerformPriceReduction(Product product)
        {
            var reduction = _priceReductionRepository.GetPriceReductionByDayOfWeek((int)DateTime.Now.DayOfWeek);

            if (reduction > 0)
            {
                ReducePrice(product, reduction);
            }
        }

        private static void ReducePrice(Product product, double reduction) 
        {
            product.PriceWithReduction -= (product.PriceWithReduction * reduction);
        }
    }
}
