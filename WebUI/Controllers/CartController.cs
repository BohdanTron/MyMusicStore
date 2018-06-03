using Domain.Abstract;
using Domain.Entities;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class CartController : Controller
    {
        private IRepository _repository;
        private IOrderProcessor _orderProcessor;

        public CartController(IRepository repository, 
            IOrderProcessor orderProcessor)
        {
            _repository = repository;
            _orderProcessor = orderProcessor;
        }

        public ViewResult CartIndex(Cart cart)
        {
            ViewBag.Products = _repository.Products;

            return View(cart);
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView(cart);
        }


        public async Task<RedirectToRouteResult> AddToCart(Cart cart, int productId)
        {
            Product product = await Task.Run(() => _repository.Products
                .FirstOrDefault(p => p.ProductId == productId));

            if (product != null)
            {
                cart.Add(product, 1);
            }

            return RedirectToAction("CartIndex");
        }

        [HttpPost]
        public async Task<RedirectToRouteResult> RemoveFromCart(Cart cart, int productId)
        {
            Product product = await Task.Run(() => _repository.Products
                .FirstOrDefault(p => p.ProductId == productId));

            if (product != null)
            {
                cart.RemoveProduct(product);
            }

            return RedirectToAction("CartIndex");
        }

        public async Task<RedirectToRouteResult> DecrementFromCart(Cart cart, int productId)
        {
            Product product = await Task.Run(() => _repository.Products
               .FirstOrDefault(p => p.ProductId == productId));

            if (product != null)
            {
                cart.DecrementProduct(product);
            }

            return RedirectToAction("CartIndex");
        }

        [Authorize]
        public ActionResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if(cart.CartLines.Count() == 0)
            {
                return RedirectToAction("CartIndex");
            }

            if (ModelState.IsValid)
            {
                _orderProcessor.ProcessOrder(cart, shippingDetails);
                cart.Clear();
                return View("Completed");
            }

            return View(shippingDetails);
        }

        public PartialViewResult AnotherProducts()
        {
            var products = _repository.Products
                .OrderByDescending(p => p.Price)
                .Take(4);

            return PartialView(products);
        }

        public PartialViewResult LatestProducts()
        {
            var products = _repository.Products
                .OrderByDescending(p => p.ProductId)
                .Take(10);

            return PartialView(products);
        }
    }
}