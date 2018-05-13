using BLink.Models.Enums;
using System;
using System.Collections.Generic;

namespace BLink.Models
{
    public class ClubEvent
    {
        public ClubEvent()
        {
            InvitedMembers = new List<ClubEventMember>();
        }

        public int Id { get; set; }

        public Club Club { get; set; }

        public EventType EventType { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public ICollection<ClubEventMember> InvitedMembers { get; set; }

        public DateTime StartTime { get; set; }

        //public string Location { get; set; } TODO 
    }
}
