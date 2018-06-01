using BLink.Models.Enums;

namespace BLink.Models.RequestModels.ClubEvents
{
    public class ClubEventFilterRequest
    {
        public int ClubId { get; set; }

        public int MemberId { get; set; }

        public EventTimeSpan EventTimeSpan { get; set; }
    }
}
