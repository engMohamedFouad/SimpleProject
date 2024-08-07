﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SimpleProject.ViewModels.Products
{
    public class UpdateProductViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "NameArIsRequired")]
        [Remote("IsProductNameArExistExcludeItself", "Product", HttpMethod = "Post", AdditionalFields = "Id", ErrorMessage = "Name Ar Is Already Exist")]
        public string NameAr { get; set; }

        [Required(ErrorMessage = "NameEnIsRequired")]
        public string NameEn { get; set; }
        [Range(1, double.MaxValue, ErrorMessage = "Min Value equal 1 and Max Value equal 500000")]
        public decimal Price { get; set; }
        [NotMapped]
        public List<IFormFile>? Files { get; set; }
        public List<string>? CurrentPaths { get; set; }
        public int CategoryId { get; set; }
    }
}
