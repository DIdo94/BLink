using BLink.Models.Enums;

namespace BLink.Models.RequestModels.Members
{
    public class PlayerFilterCriteria
    {
        public int? ClubId { get; set; }

        public string Name { get; set; }

        public double MinHeight { get; set; }

        public double MaxHeight { get; set; }

        public double MinWeight { get; set; }

        public double MaxWeight { get; set; }

        public PlayerPosition Position { get; set; }
    }
}
