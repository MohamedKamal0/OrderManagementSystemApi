using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystemApplication.Dtos.User
{
    public class RegisterRequest
    {
        public string Username { get; set; } = default!;
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;

    }
}
