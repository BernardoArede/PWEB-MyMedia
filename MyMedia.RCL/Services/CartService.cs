using MyMedia.RCL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyMedia.RCL.Services
{
    public class CartService
    {
        private List<CartItem> cartItems = new();

        public event Action? OnChange;

        public void AddToCart(Product product)
        { 
            var item = cartItems.FirstOrDefault(i => i.Product.Id == product.Id);

            if (item == null)
            {
                cartItems.Add(new CartItem { Product = product, Quantity = 1 });
            }
            else
            {
                item.Quantity++;
            }

            NotifyStateChanged();
        }
        public void RemoveFromCart(Product product)
        {
            var item = cartItems.FirstOrDefault(i => i.Product.Id == product.Id);
            if (item != null)
            {
                cartItems.Remove(item);
                NotifyStateChanged();
            }
        }
        public List<CartItem> GetCartItems()
        {
            return cartItems;
        }

        public decimal GetTotal()
        {
            return cartItems.Sum(i => i.SubTotal);
        }

        public void ClearCart()
        {
            cartItems.Clear();
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();
    }

}
