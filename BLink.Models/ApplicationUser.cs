using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace BLink.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int MemberId { get; set; }

        public Member Member { get; set; }
    }
}
