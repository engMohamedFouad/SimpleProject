namespace SimpleProject.ViewModels.Identity.Roles
{
    public class ManageRoleClaimsViewModel
    {
        public string RoleId { get; set; }
        public List<RoleClaim> RoleClaims { get; set; } = new List<RoleClaim>();
    }
}
