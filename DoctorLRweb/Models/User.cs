using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DoctorLRweb.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Role { get; set; } // Doctor or Patient

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int Age { get; set; }

        [Required]
        public long Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$",
    ErrorMessage = "Password must have at least one uppercase letter, one lowercase letter, one number, and one special character.")]
        public string Password { get; set; }
    }
}