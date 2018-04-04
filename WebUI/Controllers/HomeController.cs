using Domain.Abstract;
using System.Linq;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repository;

        public HomeController(IRepository repository)
        {
            _repository = repository;
        }

        public ViewResult HomeIndex()
        {
            HomeIndexViewModel model = new HomeIndexViewModel
            {
                AllProducts = _repository.Products
                    .Take(10),

                NewestProducts = _repository.Products
                    .OrderByDescending(p => p.ProductId)
                    .Take(3),

                SellersProducts = _repository.Products
                    .OrderByDescending(p => p.Price)
                    .Take(3)
            };

            return View(model);
        }
    }
}