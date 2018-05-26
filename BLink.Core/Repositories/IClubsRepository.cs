using BLink.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLink.Core.Repositories
{
    public interface IClubsRepository
    {
        IEnumerable<Club> GetClubs();

        IEnumerable<Club> GetClubs(Expression<Func<Club, bool>> predicate, string includeProperties = null);

        Task CreateClub(Club club);

        Task<Club> GetClubById(int clubId);

        Club GetClubByName(string name);

        void UpdateClub(Club club);

        Task SaveChangesAsync();
    }
}
