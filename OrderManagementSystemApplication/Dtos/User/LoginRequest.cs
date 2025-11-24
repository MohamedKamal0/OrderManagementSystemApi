using System.ComponentModel.DataAnnotations;

namespace OrderManagementSystemApplication.Dtos.User
{
    public class LoginRequest
    {
        [EmailAddress]
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
    }
}
