using Domain.Abstract;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<ViewResult> HomeIndex()
        {
            HomeIndexViewModel model = new HomeIndexViewModel
            {
                AllProducts = await Task.Run(() => 
                    _repository.Products
                   .Take(10)),

                NewestProducts = await Task.Run(() => 
                    _repository.Products
                    .OrderByDescending(p => p.ProductId)
                    .Take(3)),

                SellersProducts = await Task.Run(() =>
                    _repository.Products
                    .OrderByDescending(p => p.Price)
                    .Take(3))
            };

            return View(model);
        }
    }
}