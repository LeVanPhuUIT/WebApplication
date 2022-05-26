using System;
using System.ComponentModel.DataAnnotations;

namespace WebApplication.Services.User.Dto
{
    public class UpdateUserInput
    {
        [Required]
        [RegularExpression("^[a-zA-Z0-9]+$")]
        public string UserName { get; set; }
        [Required]
        public string FullName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string Email { get; set; }
        public string[] Roles { get; set; }
        public string Remark { get; set; }

        [StringLength(12, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 8)]
        public string Password { get; set; }

        [Display(Name = "Confirm new password")]
        [Compare("Password", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
