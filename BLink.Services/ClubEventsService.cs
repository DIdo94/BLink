using BLink.Core.Services;
using System.Collections.Generic;
using BLink.Models;
using BLink.Models.RequestModels.ClubEvents;
using BLink.Core.Repositories;
using System.Threading.Tasks;
using System.Linq;
using BLink.Models.Enums;

namespace BLink.Services
{
    public class ClubEventsService : IClubEventsService
    {
        private readonly IClubsRepository _clubsRepository;
        private readonly IClubEventsRepository _clubEventsRepository;

        public ClubEventsService(
            IClubsRepository clubsRepository,
            IClubEventsRepository clubEventsRepository)
        {
            _clubsRepository = clubsRepository;
            _clubEventsRepository = clubEventsRepository;
        }
        public async Task CreateEvent(ClubEventCreateRequest clubEventCreateRequest)
        {
            Club club = await _clubsRepository.GetClubById(clubEventCreateRequest.ClubId);
            if (club == null)
            {
                return;
            }

            ClubEvent clubEvent = new ClubEvent
            {
                Club = club,
                Title = clubEventCreateRequest.Title,
                Description = clubEventCreateRequest.Description,
                EventType = clubEventCreateRequest.EventType,
                StartTime = clubEventCreateRequest.StartTime,
                Longtitude = clubEventCreateRequest.Coordinates.Longtitute,
                Latitude = clubEventCreateRequest.Coordinates.Latitude
            };

            if (clubEventCreateRequest.IncludePlayerStatuses == PlayerStatus.All)
            {
                foreach (var member in club.Members)
                {
                    clubEvent.InvitedMembers.Add(new ClubEventMember
                    {
                        ClubEvent = clubEvent,
                        Member = member
                    });
                }
            }
            else
            {
                // clubEvent.InvitedMembers.Where(im => im IN IncludePlayerStatuses) = club.Members, TODO Implement individual events
            }

            _clubEventsRepository.AddEvent(clubEvent);
        }

        public async Task<bool> EditEvent(int eventId, ClubEventCreateRequest clubEventCreateRequest)
        {
            var clubEvent = await _clubEventsRepository.GetById(eventId);
            if (clubEvent == null)
            {
                return false;
            }

            clubEvent.Title = clubEventCreateRequest.Title;
            clubEvent.Description = clubEventCreateRequest.Description;
            clubEvent.EventType = clubEventCreateRequest.EventType;
            clubEvent.Latitude = clubEventCreateRequest.Coordinates.Latitude;
            clubEvent.Longtitude = clubEventCreateRequest.Coordinates.Longtitute;
            clubEvent.StartTime = clubEventCreateRequest.StartTime;

            _clubEventsRepository.UpdateEvent(clubEvent);
            return true;
        }

        public IEnumerable<ClubEventFilterResult> GetClubEvents(ClubEventFilterRequest clubEventFilterRequest)
        {
            IEnumerable<ClubEvent> clubEvents = _clubEventsRepository.GetClubEvents(clubEventFilterRequest);

            if (!clubEvents.Any())
            {
                return new List<ClubEventFilterResult>();
            }

            return clubEvents.Select(ce => new ClubEventFilterResult
            {
                Id = ce.Id,
                Title = ce.Title,
                Description = ce.Description,
                EventType = ce.EventType,
                StartTime = ce.StartTime,
                Coordinates = new ClubEventCoordinates
                {
                    Latitude = ce.Latitude,
                    Longtitute = ce.Longtitude
                }
            });
        }

        public async Task<bool> RemoveEvent(int eventId)
        {
            var clubEvent = await _clubEventsRepository.GetById(eventId);
            if (clubEvent == null)
            {
                return false;
            }

            _clubEventsRepository.RemoveEvent(clubEvent);
            return true;
        }

        public Task SaveChangesAsync()
        {
            return _clubEventsRepository.SaveChangesAsync();
        }
    }
}
