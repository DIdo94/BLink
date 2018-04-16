using BLink.Models;
using System.Collections.Generic;

namespace BLink.Core.Repositories
{
    public interface ICountriesRepository
    {
        IEnumerable<Country> GetCountries();
    }
}
