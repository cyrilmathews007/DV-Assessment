using Microservice.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microservice.Repository
{
    public interface IPriceReductionRepository
    {
        double GetPriceReductionByDayOfWeek(int dayOfWeek);
        void AddPriceReductions(List<PriceReduction> reductions);
    }
}
