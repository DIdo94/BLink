using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using BLink.Core.Repositories;
using BLink.Models;
using System.Linq;
using System.Threading.Tasks;
using BLink.Models.RequestModels.ClubEvents;
using BLink.Models.Enums;
using System.Globalization;

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

        public Task<ClubEvent> GetById(int eventId)
        {
            return _dbContext.ClubEvents.FindAsync(eventId);
        }

        public IEnumerable<ClubEvent> GetClubEvents(Expression<Func<ClubEvent, bool>> predicate)
        {
            return _dbContext.ClubEvents.Where(predicate);
        }

        public IEnumerable<ClubEvent> GetClubEvents(ClubEventFilterRequest filterRequest)
        {
            var events = _dbContext.ClubEvents.Where(ce =>
                !ce.IsDeleted &&
                ce.StartTime > DateTime.Now &&
                ce.InvitedMembers.Any(im =>
                    im.MemberId == filterRequest.MemberId &&
                    ce.Club.Id == filterRequest.ClubId));
            if (!events.Any())
            {
                return events;
            }

            switch (filterRequest.EventTimeSpan)
            {
                case EventTimeSpan.ForToday:
                    events = events.Where(e => e.StartTime.DayOfYear == DateTime.Now.DayOfYear);
                    break;
                case EventTimeSpan.ForTheWeek:
                    var weekNow = CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                            DateTime.Now,
                            CalendarWeekRule.FirstDay,
                            DayOfWeek.Monday);
                    events = events.Where(e => CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(
                                e.StartTime,
                                CalendarWeekRule.FirstDay,
                                DayOfWeek.Monday) == weekNow);
                    break;
                case EventTimeSpan.ForTheMonth:
                    events = events.Where(e => e.StartTime.Month == DateTime.Now.Month);
                    break;
                default:
                    break;
            }

            return events;
        }

        public void RemoveEvent(ClubEvent clubEvent)
        {
            clubEvent.IsDeleted = true;
            UpdateEvent(clubEvent);
        }

        public Task SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }

        public void UpdateEvent(ClubEvent clubEvent)
        {
            _dbContext.ClubEvents.Update(clubEvent);
        }
    }
}
