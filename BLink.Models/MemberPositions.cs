namespace BLink.Models
{
    public class MemberPositions
    {
        public int MemberId { get; set; }

        public virtual Member Member { get; set; }

        public int PostitionId { get; set; }

        public virtual Position Position { get; set; }
    }
}
