using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.InteropServices;

namespace BarcelonaAPI.Models
{
    public class PaymentAddresses
    {
        public int PaymentId { get; set; }
        public int UserId { get; set; }
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        [ForeignKey("UserId")]
        public virtual Users? Users { get; set; }
    }
}
