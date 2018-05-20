using System.Collections.Generic;
namespace BLink.Models
{
    public class Club
    {
        public Club()
        {
            Members = new List<Member>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual Country Country { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
