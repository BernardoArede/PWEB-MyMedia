namespace MyMedia.API.DTOs
{
    public class OrderItemDTO
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
    public class CreateOrderDto
    {
        public List<OrderItemDTO> Items { get; set; }
    }
}
