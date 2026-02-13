using System.ComponentModel.DataAnnotations;

namespace SyncSyntax.Models.ViewModels
{
    public class LoginViewModel
    {
        [Required (ErrorMessage ="Email is required field ")]
        [EmailAddress(ErrorMessage ="Email must in proper format ")]
        public string Email { get; set; }
        [Required (ErrorMessage ="Correct password required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
