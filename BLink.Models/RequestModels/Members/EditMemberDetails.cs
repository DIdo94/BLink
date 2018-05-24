using BLink.Models.Enums;
using Microsoft.AspNetCore.Http;

namespace BLink.Models.RequestModels.Members
{
    public class EditMemberDetails
    {
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        public PlayerPosition? PreferedPosition { get; set; }

        public IFormFile UserImage { get; set; }
    }
}
