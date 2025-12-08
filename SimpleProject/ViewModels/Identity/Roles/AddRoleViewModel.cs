using System.ComponentModel.DataAnnotations;

namespace SimpleProject.ViewModels.Identity.Roles
{
    public class AddRoleViewModel
    {
        [Required]
        public string Name { get; set; }
    }
}
