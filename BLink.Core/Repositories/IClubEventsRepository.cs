using BLink.Models;
using BLink.Models.RequestModels.ClubEvents;
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

        IEnumerable<ClubEvent> GetClubEvents(ClubEventFilterRequest filterRequest);

        Task<ClubEvent> GetById(int eventId);

        void UpdateEvent(ClubEvent clubEvent);

        Task SaveChangesAsync();

        void RemoveEvent(ClubEvent clubEvent);
    }
}
