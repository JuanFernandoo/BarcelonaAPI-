using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BarcelonaAPI.Models
{
    public class Products
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; } 

        [Precision(18, 2)]
        public decimal Price { get; set; }

        [ForeignKey("CategoryId")]
        public virtual Categories? Categories { get; set; }
    }
}
