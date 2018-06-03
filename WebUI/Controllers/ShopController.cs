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
using System.Threading.Tasks;

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


        public async Task<ViewResult> SearchProducts(string searchString)
        {
            ViewBag.SearchString = searchString;

            var products =  await Task.Run(() =>
                _repository.Products
                .Select(p => p));

            if (!string.IsNullOrEmpty(searchString))
            {
                products = await Task.Run(() => 
                    products.Where(p => p.Name.ToLower()
                    .Contains(searchString.ToLower())));
            }

            return View(products);
        }


        public async Task<ViewResult> ProductList()
        {
            var categories = await GetAllCategories();

            var menuList = new List<Criterion>();

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
                Products = await Task.Run(() => _repository.Products),

                CategoryList = new CriterionList
                {
                    Criterions = menuList
                }
            };

            return View(model);
        }


        [HttpPost]
        public async Task<ViewResult> ProductList(ProductListViewModel list)
        {
            ProductListViewModel model = new ProductListViewModel
            {
                Products = await SearchCheckedProducts(list),
                CategoryList = await Task.Run(() => list.CategoryList),
            };

            return View(model);
        }


        public async Task<ActionResult> SingleProductInfo(int id)
        {
            Product product = await Task.Run(() =>_repository.Products
                .FirstOrDefault(p => p.ProductId == id));

            SingleProductInfoViewModel model = new SingleProductInfoViewModel
            {
                SingleProduct = product,

                Comments = await Task.Run(() => _repository.Comments
                    .Where(c => c.ProductId == id)
                    .OrderByDescending(c => c.Date)
                    .Include(c => c.ApplicationUsers)),
                
                Users = await Task.Run(() => _repository.Users)
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


        public async Task<FileContentResult> GetImage(int productId)
        {
            Product product = await Task.Run(() => _repository.Products
                .FirstOrDefault(p => p.ProductId == productId));

            if (product != null)
            {
                return File(product.ImageData, product.ImageMimeType);
            }

            return null;
        }

        public PartialViewResult RelatedProducts(string category)
        {
            var products = _repository.Products
                    .OrderByDescending(p => p.ProductId)
                    .Where(p => p.Category == category)
                    .Take(5);

            return PartialView(products);
        }

        private async Task<IEnumerable<string>> GetAllCategories()
        {
            return await Task.Run(() => _repository.Products
                .Select(p => p.Category)
                .Distinct());
        }

        private async Task<IEnumerable<Product>> SearchCheckedProducts(ProductListViewModel list)
        {
            var result = new List<Product>();

            var products = await Task.Run(() => _repository.Products);
            var crierions = await Task.Run(() => list.CategoryList.Criterions);

            foreach (var product in products)
            {
                foreach (var crierion in crierions)
                {
                    if (crierion.IsChecked && crierion.CriterionName == product.Category)
                    {
                        result.Add(product);
                    }
                }               
            }

            return result;
        }
    }
}