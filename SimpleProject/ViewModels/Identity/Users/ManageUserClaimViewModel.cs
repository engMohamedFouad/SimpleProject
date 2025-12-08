namespace SimpleProject.ViewModels.Identity.Users
{
    public class ManageUserClaimViewModel
    {
        public string UserId { get; set; }
        public List<UserClaim> Claims { get; set; } = new List<UserClaim>();
    }
}
