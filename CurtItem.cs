namespace SmartInventory.Models
{
    public class CartItem
    {
    

        public int Id { get; set; }

        public int UserId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
        public int CartId { get; internal set; }
    }
}