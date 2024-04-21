using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Services
{
    public interface IPriceReductionService
    {
        void PerformPriceReduction(List<Product> products);
        void PerformPriceReduction(Product product);
    }
}
