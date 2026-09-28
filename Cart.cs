using System.ComponentModel.DataAnnotations;

namespace SmartInventory.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }

        public int UserId { get; set; }



    }
}
