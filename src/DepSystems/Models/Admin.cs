using System.ComponentModel.DataAnnotations;

namespace DepSystems.Models
{
    public class Admin
    {
        [Required(ErrorMessage = "Please provide your username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Please provide your password")]
        public string Password { get; set; }
    }
}
