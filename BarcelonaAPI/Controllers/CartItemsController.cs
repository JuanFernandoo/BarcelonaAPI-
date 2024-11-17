using BarcelonaAPI.Data;
using BarcelonaAPI.Dto;
using BarcelonaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Xml;
using static BarcelonaAPI.Controllers.CartItemsController;

namespace BarcelonaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartItemsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CartItemsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("items-cart")]
        [Authorize]
        public IActionResult AddCartItem([FromBody] CartItemsDTO cartItemsDTO)
        {
            if (cartItemsDTO == null || cartItemsDTO.userId <= 0 || cartItemsDTO.ProductId <= 0 || cartItemsDTO.Quantity <= 0)
            {
                return BadRequest("Por favor, asegúrate de que todos los campos sean válidos.");
            }

            var cartItem = new CartItems
            {
                userId = cartItemsDTO.userId,
                ProductId = cartItemsDTO.ProductId,
                Quantity = cartItemsDTO.Quantity
            };

            try
            {
                _context.CartItems.Add(cartItem);
                _context.SaveChanges();

                return Ok(new
                {
                    message = "Producto añadido al carrito correctamente",
                    ProductId = cartItemsDTO.ProductId,
                    Quantity = cartItemsDTO.Quantity
                });
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error al guardar el producto en el carrito: {errorMessage}");
            }
        }


        [HttpGet("cart-items-user")]
        [Authorize]

        public IActionResult GetCartItemsByUser([FromQuery] int userId)
        {
            var cartItems = _context.CartItems
                .Include(ci => ci.Product) 
                .Where(ci => ci.userId == userId)
                .Select(ci => new
                {
                    ci.CartItemId,
                    ci.ProductId,
                    ProductName = ci.Product.ProductName, 
                    ci.Quantity,
                    Price = ci.Product.Price, 
                    TotalPrice = ci.Quantity * ci.Product.Price
                })
                .ToList();

            if (!cartItems.Any())
            {
                return NotFound($"No se encontraron artículos en el carrito para el usuario con ID {userId}");
            }

            return Ok(cartItems);
        }

        [HttpPut("update-cart")]
        [Authorize]
        public async Task<IActionResult> updateCart([FromQuery] int userId, [FromBody] updateCartRequest request)
        {
            if (request == null)
            {
                return BadRequest("Ingrese los datos correspondientes");
            }

            if (userId <= 0)
            {
                return BadRequest("El ID de usuario no es válido.");
            }

            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.userId == userId && c.ProductId == request.ProductId);

            if (cartItem == null)
            {
                return NotFound("El producto no está en el carrito");
            }

            cartItem.Quantity = request.Quantity;

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Carrito actualizado correctamente",
                cartItemId = cartItem.CartItemId,
                quantity = cartItem.Quantity
            });
        }

        [HttpDelete("remove-item")]
        [Authorize]
        public async Task<IActionResult> RemoveItemFromCart(int userId, int productId)
        {
            var cartItem = await _context.CartItems
                .FirstOrDefaultAsync(c => c.userId == userId && c.ProductId == productId);

            if (cartItem == null)
            {
                return NotFound("El producto no está en el carrito");
            }

            _context.CartItems.Remove(cartItem);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Producto eliminado correctamente del carrito" });
        }

        [HttpGet("CartTotal/{userId}")]
        [Authorize]
        public IActionResult GetCartTotal(int userId)
        {
            var cartItems = _context.CartItems.Where(ci => ci.userId == userId).ToList();
            var products = _context.Products.ToList(); 

            var totalAmount = cartItems.Sum(item =>
                item.Quantity * products.FirstOrDefault(p => p.ProductId == item.ProductId)?.Price ?? 0);

            return Ok(new { totalAmount });
        }


        public class updateCartRequest 
        {
            public int ProductId { get; set; } 
            public int Quantity { get; set; }  
        }


    }
}
