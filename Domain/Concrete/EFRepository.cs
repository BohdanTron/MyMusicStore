using Domain.Abstract;
using Domain.Entities;
using System.Collections.Generic;
using System.Data.Entity;

namespace Domain.Concrete
{
    public class EFRepository : IRepository
    {
        EFDbContext _context = new EFDbContext();

        public IEnumerable<Product> Products
        {
            get { return _context.Products; }

        }

        public IDbSet<Comment> Comments
        {
            get { return _context.Comments; }
        }

        public IDbSet<ApplicationUser> Users
        {
            get { return _context.Users; }
        }

        public Product DeleteProduct(int productId)
        {
            Product dbEntry = _context.Products
                .Find(productId);

            if (dbEntry != null)
            {
                _context.Products.Remove(dbEntry);
                _context.SaveChanges();
            }

            return dbEntry;
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                _context.Products.Add(product);
            }

            else
            {
                Product dbEntry = _context.Products
                    .Find(product.ProductId);

                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Description = product.Description;
                    dbEntry.Price = product.Price;
                    dbEntry.Category = product.Category;
                    dbEntry.ImageData = product.ImageData;
                    dbEntry.ImageMimeType = product.ImageMimeType;
                }
            }

            _context.SaveChanges();            
        }

        public void AddComment(Comment comment)
        {
            if(comment != null)
            {
                _context.Comments.Add(comment);
                _context.SaveChanges();
            }
        }
    }
}
