using BLink.Models.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace BLink.Models.RequestModels.Accounts
{
    public class CreateUserViewModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password, ErrorMessage = "Паролата трябва да има поне 1 главна буква и специален символ")]
        [MinLength(6, ErrorMessage = "Паролата трябва да е поне 6 символа")]
        [MaxLength(40, ErrorMessage = "Паролата трябва да е до 40 символа")]
        public string Password { get; set; }

        [Required]
        public string ConfirmPassword { get; set; }

        [Required]
        public Role Role { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public double? Weight { get; set; }

        public double? Height { get; set; }

        [Required]
        public IFormFile UserImage { get; set; }

        public PlayerPosition? PreferedPosition { get; set; }

        public DateTime? DateOfBirth { get; set; }
    }
}
