using BLink.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace BLink.Core.Repositories
{
    public interface IClubEventsRepository
    {
        void AddEvent(ClubEvent clubEvent);

        IEnumerable<ClubEvent> GetClubEvents(Expression<Func<ClubEvent, bool>> predicate);

        Task SaveChangesAsync();
    }
}
