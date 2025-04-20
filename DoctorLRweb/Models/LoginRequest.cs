namespace DoctorLRweb.Models
{
    public class LoginRequest
    {
        public string Identifier { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}
