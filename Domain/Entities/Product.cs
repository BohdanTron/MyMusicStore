using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Domain.Entities
{
    public class Product
    {
        [HiddenInput(DisplayValue = false)]
        public int ProductId { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "Enter name of product please")]
        public string Name { get; set; }

        [DataType(DataType.MultilineText)]
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Enter description for product please")]
        public string Description { get; set; }

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Enter category for product please")]
        public string Category { get; set; }

        [Display(Name = "Price $")]
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Enter positive value for price please")]
        public decimal Price { get; set; }

        [Display(Name = "Brand")]
        [Required(ErrorMessage = "Enter brand please")]
        public string Brand { get; set; }

        public byte[] ImageData { get; set; }
        public string ImageMimeType { get; set; }

        public ICollection<Comment> Comments { get; set; }
    }
}
