using System.ComponentModel.DataAnnotations;

namespace SimpleProject.ViewModels.Categories
{
    public class AddCategoryViewModel
    {
        [Required]
        public string NameAr { get; set; }
        [Required]
        public string NameEn { get; set; }
    }
}
