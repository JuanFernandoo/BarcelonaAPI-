using BarcelonaAPI.Data;
using BarcelonaAPI.Dto;
using BarcelonaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace BarcelonaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentAddressesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentAddressesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Payment")]
        [Authorize]
        public IActionResult Payment([FromBody] PaymentAddressesDTO paymentAddressesDTO)
        {
            if (paymentAddressesDTO == null)
            {
                return BadRequest("Ingrese los campos requeridos");
            }

            var userId = int.Parse(User.FindFirst("UserId")?.Value);

            var cartItems = _context.CartItems
                .Where(ci => ci.userId == userId)
                .Include(ci => ci.Product)
                .ToList();

            if (cartItems.Count == 0)
            {
                return BadRequest("No hay productos en el carrito para realizar la compra.");
            }

            decimal totalAmount = cartItems.Sum(item => item.Quantity * item.Product.Price);

            var payment = new PaymentAddresses
            {
                CardNumber = paymentAddressesDTO.CardNumber,
                CardHolderName = paymentAddressesDTO.CardHolderName,
                ExpiryDate = paymentAddressesDTO.ExpiryDate,
                Address = paymentAddressesDTO.Address,
                City = paymentAddressesDTO.City,
                State = paymentAddressesDTO.State,
                ZipCode = paymentAddressesDTO.ZipCode,
                Country = paymentAddressesDTO.Country,
                UserId = userId
            };

            var order = new Orders
            {
                UserId = userId,
                TotalAmount = totalAmount,
                CreatedAt = DateTime.Now
            };

            try
            {
                _context.PaymentAddresses.Add(payment);
                _context.Orders.Add(order);
                _context.CartItems.RemoveRange(cartItems);
                _context.SaveChanges();

                return Ok(new
                {
                    message = "Producto comprado correctamente",
                    Address = paymentAddressesDTO.Address,
                    City = paymentAddressesDTO.City,
                    Country = paymentAddressesDTO.Country,
                    TotalAmount = totalAmount
                });
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Error al comprar el producto: {errorMessage}");
            }

        } 

        [HttpGet("Shopping")]
        [Authorize]

        public IActionResult getPayment(int userId)
        {
            var paymentAdresses = _context.PaymentAddresses
                .Where(pa => pa.UserId == userId)
                .Select(pa => new
                {
                    pa.CardNumber,
                    pa.CardHolderName,
                    pa.ExpiryDate,
                    pa.Address,
                    pa.City,
                    pa.State,
                    pa.ZipCode,
                    pa.Country

                })
                .ToList();

            if (!paymentAdresses.Any())
            {
                return NotFound($"No se pudo realizar la compra");
            }
            return Ok(paymentAdresses);

        }

    }
}
