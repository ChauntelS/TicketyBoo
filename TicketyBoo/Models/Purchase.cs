using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TicketyBoo.Models
{
    public class Purchase
    {
        //Primary key
        [Key]
        public int PurchaseId { get; set; }

        [ForeignKey("Haunt")]
        public int HauntId { get; set; }

        //navagation property 
        public Haunt? Haunt { get; set; }

        public int Quantity { get; set; } = 0;

        public string CardType { get; set; } = string.Empty;

        public int CardNum { get; set; } = 0;

        public int CardExpire { get; set; } = 0;

        public int CardSecurity { get; set; } = 0;

        public string CustomerName { get; set; } = string.Empty;

        public string CustomerEmail { get; set; } = string.Empty;

        public string CustomerBillAdd { get; set; } = string.Empty;

        public string CustomerCountry { get; set; } = string.Empty;

        public string CustomerCity { get; set; } = string.Empty;

        public DateTime CreateDate { get; set; }

        

    }
}
