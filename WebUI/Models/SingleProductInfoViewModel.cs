using Domain.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace WebUI.Models
{
    public class SingleProductInfoViewModel
    {
        public Product SingleProduct { get; set; }
        public IEnumerable<Product> RelatedProducts { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public IEnumerable<ApplicationUser> Users { get; set; }
    }
}