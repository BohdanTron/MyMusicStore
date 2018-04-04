using Domain.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace Domain.Abstract
{
    public interface IRepository
    {
        IEnumerable<Product> Products { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productId);

        IDbSet<Comment> Comments { get; }
        void AddComment(Comment comment);

        IDbSet<ApplicationUser> Users { get; }
    }
}
