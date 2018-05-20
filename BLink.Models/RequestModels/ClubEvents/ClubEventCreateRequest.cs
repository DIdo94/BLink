using BLink.Models.Enums;
using System;

namespace BLink.Models.RequestModels.ClubEvents
{
    public class ClubEventCreateRequest
    {
        public int ClubId { get; set; }

        public EventType EventType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        //public ICollection<int> InvitedMemberIds { get; set; }

        public DateTime StartTime { get; set; }

        public PlayerStatus IncludePlayerStatuses { get; set; }

        public ClubEventCoordinates Coordinates { get; set; }
    }
}
