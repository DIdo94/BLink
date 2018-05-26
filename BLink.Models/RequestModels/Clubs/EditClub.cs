using Microsoft.AspNetCore.Http;

namespace BLink.Models.RequestModels.Clubs
{
    public class EditClub
    {
        public string Name { get; set; }

        public IFormFile ClubImage { get; set; }

        public string Email { get; set; }
    }
}
