
namespace MyMedia.RCL.Models
{
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; } = 1;

        public decimal SubTotal => Product.Price * Quantity;
    }
}
