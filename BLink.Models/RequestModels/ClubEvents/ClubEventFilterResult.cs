using BLink.Models.Enums;
using System;

namespace BLink.Models.RequestModels.ClubEvents
{
    public class ClubEventFilterResult
    {
        public int Id { get; set; }

        public EventType EventType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime StartTime { get; set; }

        public ClubEventCoordinates Coordinates { get; set; }
    }
}
