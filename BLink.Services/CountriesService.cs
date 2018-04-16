using BLink.Core.Repositories;
using BLink.Core.Services;
using BLink.Models;
using System.Collections.Generic;

namespace BLink.Services
{
    public class CountriesService : ICountriesService
    {
        private readonly ICountriesRepository _countriesRepository;
        public CountriesService(ICountriesRepository countriesRepository)
        {
            _countriesRepository = countriesRepository;
        }

        public IEnumerable<Country> GetAllCountries()
        {
            throw new System.NotImplementedException();
        }
    }
}
