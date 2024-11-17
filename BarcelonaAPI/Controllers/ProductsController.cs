using BarcelonaAPI.Data;
using BarcelonaAPI.Dto;
using BarcelonaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BarcelonaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        
        public ProductsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("Add-Product")]
        [Authorize]

        public IActionResult AddProduct([FromBody] CreateProductDTO productDTO)
        {
            if (productDTO == null || string.IsNullOrEmpty(productDTO.ProductName) || productDTO.Price <= 0)
            {
                return BadRequest("Ingrese los campos requeridos");
            }

            var categoryExists = _context.Categories.Any(c => c.CategoryId == productDTO.CategoryId);
            if (!categoryExists)
            {
                return BadRequest("La categoria no existe");
            }

            var product = new Products
            {
                ProductName = productDTO.ProductName,
                Description = productDTO.Description,
                CategoryId = productDTO.CategoryId,
                Price = productDTO.Price
            };

            try
            {
                _context.Products.Add(product);
                _context.SaveChanges();

                return Ok(new
                {
                    message = "Product creado correctamente",
                    productName = product.ProductName,
                    productDescription = product.Description,
                    productPrice = product.Price
                });
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, $"Erro al guardar el producto: {errorMessage}");
            }
        }


        [HttpGet("list-products-by-category")]
        [Authorize]

        public IActionResult GetProduct(int CategoryId)
        {
            var product = _context.Products
                .Include(p => p.Categories)
                .Where(p => p.Categories.CategoryId == CategoryId)
                .ToList();

            if (!product.Any())
            {
                return NotFound($"No se encontraron productos para la categoria que se asigno");
            }

            var productsDTO = product.Select(p => new
            {
                productID = p.ProductId,
                productName = p.ProductName,
                productDescription = p.Description,
                productPrice = p.Price,
                Categories = new
                {
                    CategoryId = p.Categories.CategoryId,
                    categoryName = p.Categories.CategoryName
                }
            }).ToList();

            return Ok(productsDTO);

        }
    }
}
