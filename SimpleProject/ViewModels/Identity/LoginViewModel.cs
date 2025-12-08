using System.ComponentModel.DataAnnotations;

namespace SimpleProject.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter your UserName")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Please Enter your Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
        public string? ReturnUrl { get; set; }
    }
}
