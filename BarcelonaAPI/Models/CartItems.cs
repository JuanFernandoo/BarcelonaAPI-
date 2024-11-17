namespace BarcelonaAPI.Models
{
    public class CartItems
    {
        public int CartItemId { get; set; }
        public int userId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public virtual Products Product { get; set; }

    }
}
