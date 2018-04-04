using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<Comment> Comments { get; set; }

        public ApplicationUser()
        {
            Comments = new List<Comment>();
        }
    }
}
