using Domain.Entities;
using System.Collections.Generic;

namespace WebUI.Models
{
    public class HomeIndexViewModel
    {
        public IEnumerable<Product> AllProducts { get; set; }
        public IEnumerable<Product> NewestProducts { get; set; }
        public IEnumerable<Product> SellersProducts { get; set; }
    }
}