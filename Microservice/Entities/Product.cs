using MongoDB.Bson;
using System;

namespace Microservice.Entities
{
    public class Product : MongoObject
    {
        public string Name { get; set; }
        public DateTime EntryDate { get; set; }
        public double Price { get; set; }     
    }
}
