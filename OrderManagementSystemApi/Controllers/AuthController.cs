using Microsoft.AspNetCore.Mvc;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.User;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;


namespace OrderManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService _userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<ActionResult<ApiResponse<User>>> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _userService.RegisterAsync(model);

            return response;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _userService.GetTokenAsync(model);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return BadRequest(response);

            var token = _userService.GenerateJwtToken(response.Data);

            return Ok(new ApiResponse<string>
            {
                StatusCode = System.Net.HttpStatusCode.OK,
                Data = token,
                Message = "Login successful"
            });
        }
    }
}


