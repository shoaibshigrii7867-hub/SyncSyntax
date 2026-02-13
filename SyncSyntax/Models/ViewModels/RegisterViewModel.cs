using System.ComponentModel.DataAnnotations;

namespace SyncSyntax.Models.ViewModels
{
    public class RegisterViewModel
    {
        [Required (ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage ="email should Be in Proper Format")]
        public string Email { get; set; }

        [Required (ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Compare("Password", ErrorMessage = "Password and Confirm Password do not match")]

        public string ConfirmPassword { get; set; }
    }
}
