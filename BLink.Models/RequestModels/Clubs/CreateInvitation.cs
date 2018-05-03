using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace BLink.Models.RequestModels.Clubs
{
    public class CreateInvitation
    {
        //[Required]
        public int PlayerId { get; set; }

        //[Required]
        public string Description { get; set; }
    }
}
