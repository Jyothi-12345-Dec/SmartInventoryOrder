using System.ComponentModel.DataAnnotations;

namespace SmartInventory.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        [MinLength(2)]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        public string Password { get; set; }

        public string Role { get; set; }
    }
}