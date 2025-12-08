using SimpleProject.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SimpleProject.Models.Identity
{
    public class Claim : LocalizableEntity
    {
        [Key]
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public bool IsUserClaim { get; set; } = true;
        public bool IsRoleClaim { get; set; } = true;
    }
}
