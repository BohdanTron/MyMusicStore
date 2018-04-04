using Domain.Abstract;
using Domain.Entities;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebUI.Controllers
{
    public class AdminController : Controller
    {
        IRepository _repository;

        public AdminController(IRepository repository)
        {
            _repository = repository;
        }

        public ViewResult AdminIndex()
        {
            return View(_repository.Products);
        }

        public ViewResult Create()
        {
            return View("Edit", new Product());
        }

        public ViewResult Edit(int productId)
        {
            Product product = _repository.Products
                .FirstOrDefault(p => p.ProductId == productId);

            return View(product);
        }

        [HttpPost]
        public ActionResult Edit(Product product, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                if(image != null)
                {
                    product.ImageMimeType = image.ContentType;
                    product.ImageData = new byte[image.ContentLength];
                    image.InputStream.Read(product.ImageData, 0, image.ContentLength);
                }

                _repository.SaveProduct(product);
                TempData["message"] = string.Format("Changes in the product \"{0}\" were saved", product.Name);
                return RedirectToAction("AdminIndex");
            }

            return View(product);
        }

        [HttpPost]
        public ActionResult Delete(int productId)
        {
            Product deletedProduct = _repository.DeleteProduct(productId);

            if (deletedProduct != null)
            {
                TempData["message"] = string.Format("Product \"{0}\" was deleted",
                    deletedProduct.Name);
            }
            return RedirectToAction("AdminIndex");
        }
    }
}