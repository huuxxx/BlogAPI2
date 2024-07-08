using System.ComponentModel.DataAnnotations;

namespace BlogAPI2.Contracts
{
    public class RegisterRequest
    {
        [Required]
        public string Username { get; set; }

        [EmailAddress, Required]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
