using Domain.Abstract;
using Domain.Entities;
using System.Collections.Generic;
using System.Linq;
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


        public RedirectToRouteResult AddToCart(Cart cart, int productId)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                cart.Add(product, 1);
            }

            return RedirectToAction("CartIndex");
        }

        [HttpPost]
        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if (product != null)
            {
                cart.RemoveProduct(product);
            }

            return RedirectToAction("CartIndex");
        }

        public RedirectToRouteResult DecrementFromCart(Cart cart, int productId)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            if(product != null)
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
            IEnumerable<Product> products = _repository.Products
                .OrderByDescending(p => p.Price)
                .Take(4);

            return PartialView(products);
        }

        public PartialViewResult LatestProducts()
        {
            IEnumerable<Product> products = _repository.Products
                .OrderByDescending(p => p.ProductId)
                .Take(10);

            return PartialView(products);
        }
    }
}