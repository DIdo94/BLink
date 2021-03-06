﻿using Microsoft.AspNetCore.Http;

namespace BLink.Models.RequestModels.Clubs
{
    public class CreateClubModel
    {
        public string Email { get; set; }

        public string Name { get; set; }

        public IFormFile ClubImage { get; set; }
    }
}
