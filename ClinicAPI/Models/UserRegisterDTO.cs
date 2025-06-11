namespace ClinicAPI.Models
{
    public class UserRegisterDto
    {
        public string Username { get; set; } // ✅ Added Username
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } = "User"; // ✅ Default role is "User"
    }
}
