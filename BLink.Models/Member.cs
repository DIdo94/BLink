using System;
using System.Collections.Generic;

namespace BLink.Models
{
    // TODO Add Role column despite data repetition
    public class Member
    {
        public Member()
        {
            MemberPositions = new List<MemberPositions>();
        }

        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhotoPath { get; set; }

        public string PhotoThumbnailPath { get; set; }

        public Country Country { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public virtual ApplicationUser IdentityUser { get; set; }

        public virtual Club Club { get; set; }

        public ICollection<MemberPositions> MemberPositions { get; set; }

        public ICollection<ClubEventMember> ClubEvents { get; set; }
    }
}