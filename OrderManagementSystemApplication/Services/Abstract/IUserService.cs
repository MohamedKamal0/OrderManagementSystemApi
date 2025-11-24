using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.User;
using OrderManagementSystemDomain.Models;

namespace OrderManagementSystemApplication.Services.Abstract
{
    public interface IUserService
    {
        Task<ApiResponse<User>> GetTokenAsync(LoginRequest model);
        Task<ApiResponse<User>> RegisterAsync(RegisterRequest model);
        string GenerateJwtToken(User user); // Add this line

    }
}
