using BLink.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace BLink.Models.RequestModels.Accounts
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5)]
        [MaxLength(40)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(5)]
        [MaxLength(40)]
        public string ConfirmPassword { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }
    }
}
