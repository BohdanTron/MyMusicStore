using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public int? ProductId { get; set; }

        [ForeignKey("ProductId")]
        public Product Product { get; set; }

        public ICollection<ApplicationUser> ApplicationUsers { get; set; }

        public Comment()
        {
            ApplicationUsers = new List<ApplicationUser>();
        }
    }
}
