using Domain.Concrete;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Net;
using WebUI.Infrastructure;

namespace WebUI.Controllers
{
    public class AccountController : Controller
    { 
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext()
                    .GetUserManager<ApplicationUserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }


        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Name,
                    Email = model.Email,
                    EmailConfirmed = false
                };

                IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    MailAddress from = new MailAddress("tronbodya@gmail.com", "Web Registration");
                    MailAddress to = new MailAddress(user.Email);

                    MailMessage message = new MailMessage(from, to)
                    {
                        Subject = "Email confirmation",
                        Body = string.Format("To complete the registration, click on the link:" +
                                    "<a href=\"{0}\" title=\"Confirm registration\">{0}</a>",
                                    Url.Action("ConfirmEmail", "Account", 
                                    new { Token = user.Id, user.Email }, 
                                    Request.Url.Scheme)),
                        IsBodyHtml = true
                    };

                    SmtpClient smtp = new SmtpClient()
                    {
                        EnableSsl = true,
                        Port = 587,
                        Host = "smtp.gmail.com",
                        UseDefaultCredentials = false,
                        Credentials = new NetworkCredential("tronbodya@gmail.com", "Bohdan301127")
                    };


                    smtp.Send(message);

                    return RedirectToAction("Confirm", "Account", new { user.Email });
                }
                else
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }
            }
            return View(model);
        }


        public ViewResult Confirm(string email)
        {
            return View(email);
        }

        public async Task<ActionResult> ConfirmEmail(string token, string email)
        {
            ApplicationUser user = UserManager.FindById(token);
            if (user != null)
            {
                if (user.Email == email)
                {
                    user.EmailConfirmed = true;
                    await UserManager.UpdateAsync(user);

                    ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);

                    return RedirectToAction("HomeIndex", "Home");
                }
                else
                {
                    return RedirectToAction("Confirm", "Account", new { user.Email });
                }
            }
            else
            {
                return RedirectToAction("Confirm", "Account", new { email = "" });
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await UserManager
                    .FindByNameOrEmailAsync(model.NameOrEmail, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Invalid login or password.");
                }
                else
                {
                    if (user.EmailConfirmed == true)
                    {
                        ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                                        DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("HomeIndex", "Home");
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError("", "Not verified email.");
                    }

                }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("HomeIndex", "Home");
        }
    }
}