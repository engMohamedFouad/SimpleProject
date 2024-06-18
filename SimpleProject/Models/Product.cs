using SimpleProject.Helpers;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Models
{
    public class Product : LocalizableEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public decimal Price { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
        public virtual ICollection<ProductImages> ProductsImages { get; set; } = new HashSet<ProductImages>();
    }
}
