using Microsoft.EntityFrameworkCore;

namespace BarcelonaAPI.Models
{
    public class Orders
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
       
        [Precision(18, 2)]
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } 
    }
}
