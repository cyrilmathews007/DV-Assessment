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

        public double GetPriceReductionByDayOfWeek(int dayOfWeek)
        {
            return _reductionCollection.Find(reduction => reduction.DayOfWeek == dayOfWeek).FirstOrDefault()?.Reduction ?? 0;
        }

        public void AddPriceReductions(List<PriceReduction> reductions)
        {
            _reductionCollection.InsertMany(reductions);
        }
    }
}
