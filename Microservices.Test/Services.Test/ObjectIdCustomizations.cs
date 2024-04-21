using AutoFixture;

namespace Microservices.Test.Services.Test
{
    public class ObjectIdCustomizations
    {
        public class ProductObjectIdCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<Microservice.Entities.Product>(composer => composer.With(p => p._id, new MongoDB.Bson.ObjectId()));
            }
        }

        public class PriceReductionObjectIdCustomization : ICustomization
        {
            public void Customize(IFixture fixture)
            {
                fixture.Customize<Microservice.Entities.PriceReduction>(composer => composer.With(p => p._id, new MongoDB.Bson.ObjectId()));
            }
        }
    }
}
