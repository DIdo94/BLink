using BLink.Models.RequestModels.ClubEvents;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BLink.Core.Services
{
    public interface IClubEventsService
    {
        Task CreateEvent(ClubEventCreateRequest clubEventCreateRequest);

        IEnumerable<ClubEventFilterResult> GetClubEvents(ClubEventFilterRequest clubEventFilterRequest);

        Task SaveChangesAsync();
    }
}
