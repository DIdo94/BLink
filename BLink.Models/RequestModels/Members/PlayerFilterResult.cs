using System;
using BLink.Models.Enums;

namespace BLink.Models.RequestModels.Members
{
    public class PlayerFilterResult
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public int PositionId { get; set; }

        public PlayerPosition? PreferedPosition { get; set; }

        public int? ClubId { get; set; }

        public string Thumbnail { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
