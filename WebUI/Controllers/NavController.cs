using System.Collections.Generic;
using System.Web.Mvc;
using WebUI.Models;

namespace WebUI.Controllers
{
    public class NavController : Controller
    {
        public PartialViewResult Menu(string selectedLink)
        {
            MenuViewModel model = new MenuViewModel
            {
                MenuLinks = new List<string>
                {
                    "Home", "Shop", "Cart", "Contact"
                },

                AccountLinks = new List<string>
                {
                    "Log in", "Register"
                },

                SelectedLink = selectedLink
            };

            return PartialView(model);
        }
    }
}