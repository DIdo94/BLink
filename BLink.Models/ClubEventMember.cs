namespace BLink.Models
{
    public class ClubEventMember
    {
        public int ClubEventId { get; set; }

        public ClubEvent ClubEvent { get; set; }

        public int MemberId { get; set; }

        public Member Member { get; set; }
    }
}
