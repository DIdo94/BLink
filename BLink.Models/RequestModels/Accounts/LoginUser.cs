using System.ComponentModel.DataAnnotations;

namespace BLink.Models.RequestModels.Accounts
{
    public class LoginUser
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5)]
        [MaxLength(40)]
        public string Password { get; set; }
    }
}
