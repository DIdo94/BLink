using BLink.Models.Enums;

namespace BLink.Models
{
    public class Invitation
    {
        public int Id { get; set; }

        public string Description { get; set; }

        public Member InvitedPlayer { get; set; }

        public Club InvitingClub { get; set; }

        public InvitationStatus Status { get; set; }
    }
}
