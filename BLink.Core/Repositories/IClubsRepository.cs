using BLink.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BLink.Models.RequestModels.Members;

namespace BLink.Core.Repositories
{
    public interface IClubsRepository
    {
        IEnumerable<Club> GetClubs();

        IEnumerable<Club> GetClubs(Expression<Func<Club, bool>> predicate, string includeProperties = null);

        Task CreateClub(Club club);

        Task SaveChangesAsync();
    }
}
