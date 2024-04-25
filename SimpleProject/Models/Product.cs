using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.Models
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        [Remote("IsProductNameExist", "Product", HttpMethod = "Post", ErrorMessage = "Name Is Already Exist")]
        public string Name { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Min Value equal 1 and Max Value equal 500000")]
        public decimal Price { get; set; }
        [NotMapped]
        public IFormFile? File { get; set; }
        public string? Path { get; set; }

        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }

    }
}
