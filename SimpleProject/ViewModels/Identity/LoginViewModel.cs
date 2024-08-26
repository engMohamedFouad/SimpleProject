using System.ComponentModel.DataAnnotations;

namespace SimpleProject.ViewModels.Identity
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter your Email")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Please Enter your Password")]
        public string Password { get; set; }
        public bool RememberMe { get; set; } = false;
    }
}
