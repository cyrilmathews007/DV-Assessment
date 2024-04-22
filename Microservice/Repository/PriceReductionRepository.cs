using Microservice.Entities;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Repository
{
    public class PriceReductionRepository : IPriceReductionRepository
    {
        private readonly IMongoCollection<PriceReduction> _reductionCollection;

        public PriceReductionRepository(IMongoClient mongoClient)
        {
            _reductionCollection = mongoClient.GetDatabase("deli_veggie").GetCollection<PriceReduction>("price-reduction");
        }

        public async Task<double> GetPriceReductionByDayOfWeekAsync(int dayOfWeek)
        {
            var reduction = await _reductionCollection.Find(reduction => reduction.DayOfWeek == dayOfWeek).FirstOrDefaultAsync();
            return reduction?.Reduction ?? 0;
        }

        public async Task AddPriceReductionsAsync(List<PriceReduction> reductions)
        {
            await _reductionCollection.InsertManyAsync(reductions);
        }
    }
}
