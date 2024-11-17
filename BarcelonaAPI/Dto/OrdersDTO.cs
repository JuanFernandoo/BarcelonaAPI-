using Microsoft.EntityFrameworkCore;

namespace BarcelonaAPI.Dto
{
    public class OrdersDTO
    {

        public int OrderId { get; set; }
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }

    }
}
