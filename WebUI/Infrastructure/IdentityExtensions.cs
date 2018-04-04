using Domain.Concrete;
using Domain.Entities;
using Microsoft.AspNet.Identity;
using System.Security.Principal;
using System.Threading.Tasks;

namespace WebUI.Infrastructure
{
    public static class IdentityExtensions
    {
        public static async Task<ApplicationUser> FindByNameOrEmailAsync
            (this UserManager<ApplicationUser> userManager, string userNameOrEmail, string password)
        {
            var userName = userNameOrEmail;

            if (userName.Contains("@"))
            {
                var userForEmail = await userManager.FindByEmailAsync(userNameOrEmail);
                if(userForEmail != null)
                {
                    userName = userForEmail.UserName;
                }
            }

            return await userManager.FindAsync(userName, password);
        }


        public static string GetUserEmail(this IIdentity identity)
        {
            var userId = identity.GetUserId();
            using (var context = new EFDbContext())
            {
                var user = context.Users.Find(userId);
                return user.Email;
            }
        }
    }
}