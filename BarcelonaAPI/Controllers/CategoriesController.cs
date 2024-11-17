using Azure.Core;
using BarcelonaAPI.Data;
using BarcelonaAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BarcelonaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoriesController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("create-category")]
        [Authorize]

        public IActionResult CreateCategory([FromBody] Categories categories)
        {
            if (categories == null)
            {
                return BadRequest("Ingrese los datos requeridos");
            }



            _context.Categories.Add(categories);
            _context.SaveChanges();

            return Ok(new 
            {
                message = "Categoria creada exitosamente",
                categoryName = categories.CategoryName
            });
        }
        [HttpGet("full-categories")]  
        [Authorize] 
        public IActionResult GetCategories()
        {
            var categories = _context.Categories.ToList();
            if (!categories.Any())
            {
                return NotFound(); 
            }

            return Ok(categories); 
        }
    }
}
