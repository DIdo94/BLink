using BLink.Models;
using System.Collections.Generic;

namespace BLink.Core.Services
{
    public interface ICountriesService
    {
        IEnumerable<Country> GetAllCountries();
    }
}
