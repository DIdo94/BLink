using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BLink.Core.Repositories;
using BLink.Models;
using System.Linq;
using System.Threading.Tasks;

namespace BLink.Data.Repositories
{
    public class ClubEventsRepository : IClubEventsRepository
    {
        private readonly BlinkDbContext _dbContext;
        public ClubEventsRepository(BlinkDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddEvent(ClubEvent clubEvent)
        {
            _dbContext.ClubEvents.Add(clubEvent);
        }

        public IEnumerable<ClubEvent> GetClubEvents(Expression<Func<ClubEvent, bool>> predicate)
        {
            return _dbContext.ClubEvents.Where(predicate);
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }
    }
}
