using System.Collections.Generic;
using System.Linq;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> _lineCollection = new List<CartLine>();

        public IEnumerable<CartLine> CartLines
        {
            get { return _lineCollection; }
        }

        public void Add(Product product, int quantity)
        {
            CartLine cartLine = _lineCollection
                .FirstOrDefault(p => p.Product.ProductId == product.ProductId);

            if (cartLine == null)
            {
                _lineCollection.Add(new CartLine
                {
                    Product = product,
                    Quantity = quantity
                });
            }
            else
            {
                cartLine.Quantity += quantity;
            }
        }

        public void RemoveProduct(Product product)
        {
            _lineCollection.RemoveAll(p => p.Product.ProductId == product.ProductId);
        }

        public void DecrementProduct(Product product)
        {
            CartLine cartLine = _lineCollection
                .FirstOrDefault(p => p.Product.ProductId == product.ProductId);

            if(cartLine.Quantity > 1)
            {
                cartLine.Quantity -= 1;

            }

            else
            {
                RemoveProduct(product);
            }
        }

        public decimal ComputeTotalValue()
        {
            return _lineCollection.Sum(p => p.Product.Price * p.Quantity);
        }

        public void Clear()
        {
            _lineCollection.Clear();
        }
    }


    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
