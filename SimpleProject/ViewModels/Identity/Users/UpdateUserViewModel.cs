using System.ComponentModel.DataAnnotations;

namespace SimpleProject.ViewModels.Identity.Users
{
    public class UpdateUserViewModel
    {
        [Required]
        public string Id { get; set; }
        [Required]
        public string NameAr { get; set; }
        [Required]
        public string NameEn { get; set; }
        [Required]
        public string? Address { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}
