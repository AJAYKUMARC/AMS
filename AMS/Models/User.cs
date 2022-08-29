using System.ComponentModel.DataAnnotations;

namespace AMS.Models
{
    public class User: BaseModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
