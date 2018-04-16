using System.Collections.Generic;

namespace BLink.Models
{
    public class Position
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<MemberPositions> MemberPositions { get; set; }
    }
}