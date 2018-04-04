using Domain.Abstract;
using Domain.Concrete;
using Domain.Entities;
using Microsoft.AspNet.Identity.Owin;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using WebUI.Models;
using System;

namespace WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IRepository _repository;

        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
        }

        public ShopController(IRepository repository)
        {
            _repository = repository;
        }


        public ViewResult SearchProducts(string searchString)
        {
            ViewBag.SearchString = searchString;

            IEnumerable<Product> products = _repository.Products
                .Select(p => p);

            if (!string.IsNullOrEmpty(searchString))
            {
                products = products.Where(p => p.Name.ToLower()
                    .Contains(searchString.ToLower()));
            }

            return View(products);
        }


        public ViewResult ProductList()
        {
            IEnumerable<string> categories = GetAllCategories();

            List<Criterion> menuList = new List<Criterion>();

            foreach (var category in categories)
            {
                menuList.Add(new Criterion
                {
                    CriterionName = category,
                    IsChecked = false
                });
            }


            ProductListViewModel model = new ProductListViewModel
            {
                Products = _repository.Products,
                CategoryList = new CriterionList
                {
                    Criterions = menuList
                }
            };


            return View(model);
        }


        [HttpPost]
        public ViewResult ProductList(ProductListViewModel list)
        {
            ProductListViewModel model = new ProductListViewModel
            {
                Products = SearchCheckedProducts(list),
                CategoryList = list.CategoryList,
            };

            return View(model);
        }


        public ActionResult SingleProductInfo(int id)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == id);

            SingleProductInfoViewModel model = new SingleProductInfoViewModel
            {
                SingleProduct = product,

                Comments = _repository.Comments
                    .Where(c => c.ProductId == id)
                    .OrderByDescending(c => c.Date)
                    .Include(c => c.ApplicationUsers),
                
                Users = _repository.Users
            };

            return View(model);
        }

        [Authorize]
        public RedirectToRouteResult CommentOn(int productId, string userId, string review)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            ApplicationUser user = _repository.Users 
                .FirstOrDefault(u => u.Id == userId);

            Comment comment = new Comment
            {
                Text = review,
                ProductId = product.ProductId,
                Product = product,
                Date = DateTime.Now
            };
            comment.ApplicationUsers.Add(user);

            _repository.AddComment(comment);

            return RedirectToAction("SingleProductInfo", new { id = productId });
        }


        public FileContentResult GetImage(int productId)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }

            return null;
        }

        public PartialViewResult RelatedProducts(string category)
        {
            IEnumerable<Product> products = _repository.Products
                    .OrderByDescending(p => p.ProductId)
                    .Where(p => p.Category == category)
                    .Take(5);

            return PartialView(products);
        }

        private IEnumerable<string> GetAllCategories()
        {
            return _repository.Products
                .Select(p => p.Category)
                .Distinct();
        }

        private IEnumerable<Product> SearchCheckedProducts(ProductListViewModel list)
        {
            List<Product> result = new List<Product>();

            foreach (var product in _repository.Products)
            {
                foreach (var category in list.CategoryList.Criterions)
                {
                    if (category.IsChecked && category.CriterionName == product.Category)
                    {
                        result.Add(product);
                    }
                }               
            }

            return result;
        }
    }
}