using System.ComponentModel.DataAnnotations;

namespace SimpleProject.ViewModels.Identity.Claims
{
    public class AddClaimViewModel
    {
        [Required]
        public string NameAr { get; set; }
        [Required]
        public string NameEn { get; set; }
        public bool IsUserClaim { get; set; } = true;
        public bool IsRoleClaim { get; set; } = true;
    }
}
