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

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _productCollection.Find(Builders<Product>.Filter.Empty).ToListAsync();
        }

        public async Task<Product> GetProductAsync(string id)
        {
            if (ObjectId.TryParse(id, out var objectId))
            {
                return await _productCollection.Find(document => document._id == objectId).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task AddProductsAsync(List<Product> products)
        {
            await _productCollection.InsertManyAsync(products);
        }

        public async Task<bool> HasRecordsAsync()
        {
            var count = await _productCollection.CountDocumentsAsync(Builders<Product>.Filter.Empty);
            return count > 0;
        }
    }
}
