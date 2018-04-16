using BLink.Core.Repositories;
using BLink.Models;
using System.Collections.Generic;

namespace BLink.Data.Repositories
{
    public class CountriesRepository : ICountriesRepository
    {
        private readonly BlinkDbContext _dbContext;
        public CountriesRepository(BlinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IEnumerable<Country> GetCountries()
        {
            return _dbContext.Countries;
        }
    }
}
