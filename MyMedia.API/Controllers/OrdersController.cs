using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyMedia.API.Data;
using MyMedia.API.DTOs;
using MyMedia.API.Entities;
using System.Security.Claims;

namespace MyMedia.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Só utilizadores com login feito podem encomendar 
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDto model)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null) return Unauthorized();

            var order = new Order
            {
                UserId = userId,
                Date = DateTime.Now,
                Status = "Pendente",
                Items = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var itemDto in model.Items)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);
                if (product == null) continue;

                var orderItem = new OrderItem
                {
                    ProductId = product.Id,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                order.Items.Add(orderItem);
                total += orderItem.UnitPrice * orderItem.Quantity;
            }

            order.Total = total;
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Encomenda criada com sucesso!", orderId = order.Id });
        }

        [HttpGet("my-orders")]
        public async Task<ActionResult<IEnumerable<Order>>> GetMyOrders()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            var orders = await _context.Orders
                .Include(o => o.Items)
                .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.Date) 
                .ToListAsync();

            return Ok(orders);
        }


    }
}