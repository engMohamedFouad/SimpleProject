using SimpleProject.Helpers;
using System.ComponentModel.DataAnnotations;

namespace SimpleProject.Models
{
    public class Category : LocalizableEntity
    {
        [Key]
        public int Id { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
    }
}
