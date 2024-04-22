// See https://aka.ms/new-console-template for more information
using System.Threading.Tasks;

namespace Microservice.Seed
{
    public interface ISeedService
    {
        public Task PopulateDataAsync();
    }
}