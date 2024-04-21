using Microservice.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Repository
{
    public class ProductRepository : IProductRepository
    {

        private readonly IMongoCollection<Product> _productCollection;

        public ProductRepository(IMongoClient mongoClient)
        {
            _productCollection = mongoClient.GetDatabase("deli_veggie").GetCollection<Product>("product");
        }

        public List<Product> GetProducts()
        {
            return _productCollection.Find(Builders<Product>.Filter.Empty).ToList();
        }

        public Product GetProduct(string id)
        {
            if (ObjectId.TryParse(id, out var objectId))
            {
                return _productCollection.Find(document => document._id == objectId).FirstOrDefault();
            }
            else
            {
                return null;
            }
        }

        public void AddProducts(List<Product> products)
        {
            _productCollection.InsertMany(products);
        }

        public bool HasRecords()
        {
            return _productCollection.CountDocuments(Builders<Product>.Filter.Empty) > 0;
        }
    }
}
