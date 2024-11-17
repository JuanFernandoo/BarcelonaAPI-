using BarcelonaAPI.Data;
using BarcelonaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarcelonaAPI.Dto
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext _context; 

        public OrdersController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Create-order")]
        [Authorize]

        public IActionResult CreateOrder([FromBody] OrdersDTO ordersDTO)
        {
            if (ordersDTO == null)
            {
                return BadRequest("Ingrese los campos requeridos");
            }

            var cartItems = _context.CartItems
                .Where(ci => ci.userId == ordersDTO.UserId)
                .ToList();

            if (!cartItems.Any())
            {
                return BadRequest("No hay productos en el carrito");
            }

            decimal total= cartItems.Sum(ci => ci.Product.Price * ci.Quantity);

            var order = new Orders
            {
                UserId = ordersDTO.UserId,
                TotalAmount = total,
                CreatedAt = DateTime.Now,
            };

            try
            {
                _context.Orders.Add(order);
                _context.SaveChanges();

                return Ok(new
                {
                    message = "Orden creada con exito",
                    orderId = order.OrderId,
                    total = order.TotalAmount
                });
            } catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error creating order: {errorMessage}");
            }
        }

        [HttpGet("Orders-User-Id")]
        [Authorize]

        public IActionResult getOrder(int userId)
        {
            var orders = _context.Orders
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt) 
                .ToList();

            if (!orders.Any())
            {
                return NotFound("El usuario no tiene ordenes activas");
            }
            var ordersDTO = orders.Select(o => new OrdersDTO
            {
                OrderId = o.OrderId,
                UserId = o.UserId,
                TotalAmount = o.TotalAmount,
                CreatedAt = o.CreatedAt
            }).ToList();

            return Ok(ordersDTO);

        }
    }

}
