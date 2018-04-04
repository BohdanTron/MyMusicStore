using Domain.Entities;
using System.Collections.Generic;

namespace WebUI.Models
{
    public class ProductListViewModel
    {
        public IEnumerable<Product> Products { get; set; }
        public CriterionList CategoryList { get; set; }
    }
}