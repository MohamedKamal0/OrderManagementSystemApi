using Microsoft.AspNetCore.Mvc;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.User;
using OrderManagementSystemApplication.Services.Abstract;


namespace OrderManagementSystemAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController(IUserService _userService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _userService.RegisterAsync(model);

            return Ok("User registered successfully.");
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<LoginRequest>>> Login([FromBody] LoginRequest model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _userService.GetTokenAsync(model);
            if (response.StatusCode != System.Net.HttpStatusCode.OK)
                return BadRequest(response);

            var token = _userService.GenerateJwtToken(response.Data);


            return Ok(token);
        }
    }
}


