using MongoDB.Bson;
using System;

namespace Microservice.Entities
{
    public class PriceReduction : MongoObject
    {
        public int DayOfWeek { get; set; }
        public double Reduction { get; set; }
    }
}
