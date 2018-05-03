using BLink.Core.Repositories;
using BLink.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace BLink.Data.Repositories
{
    public class ClubsRepository : IClubsRepository
    {
        private readonly BlinkDbContext _dbContext;
        public ClubsRepository(BlinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task CreateClub(Club club)
        {
            return _dbContext.Clubs.AddAsync(club);
        }

        public Task<Club> GetClubById(int clubId)
        {
            return _dbContext.Clubs.FindAsync(clubId);
        }

        public IEnumerable<Club> GetClubs()
        {
            return _dbContext.Clubs;
        }

        public IEnumerable<Club> GetClubs(Expression<Func<Club, bool>> predicate, string includeProperties = null)
        {
            if (!string.IsNullOrWhiteSpace(includeProperties))
            {
                return _dbContext.Clubs.Include(includeProperties).Where(predicate);
            }

            return _dbContext.Clubs.Where(predicate);
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
