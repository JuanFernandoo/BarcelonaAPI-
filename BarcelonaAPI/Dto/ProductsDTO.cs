using Microsoft.EntityFrameworkCore;

namespace BarcelonaAPI.Dto
{
    public class CreateProductDTO
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public int CategoryId { get; set; }

        [Precision(18, 2)]
        public decimal Price { get; set; }
    }
}
