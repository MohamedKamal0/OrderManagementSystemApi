using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using OrderManagementSystemApplication.BaseResponse;
using OrderManagementSystemApplication.Dtos.User;
using OrderManagementSystemApplication.Hashing;
using OrderManagementSystemApplication.Services.Abstract;
using OrderManagementSystemDomain.Models;
using OrderManagementSystemDomain.Repositories;

namespace OrderManagementSystemApplication.Services.Implemntation
{
    public class UserService(IUserRepository _userRepository,
        ResponseHandler _responseHandler, PasswordHasher _passwordHasher, IConfiguration _configuration) : IUserService
    {
        public async Task<ApiResponse<User>> GetTokenAsync(LoginRequest model)
        {

            var user = await _userRepository.GetTableAsTracking().FirstOrDefaultAsync(u => u.Email == model.Email);

            if (user == null || (!_passwordHasher.Verify(model.Password, user.PasswordHash)))
                return _responseHandler.BadRequest<User>("Invalid email or password.");

            return _responseHandler.Success(user);
        }

        public async Task<ApiResponse<User>> RegisterAsync(RegisterRequest model)
        {
            if (_userRepository.GetTableNoTracking().Any(u => u.Email == model.Email || u.Username == model.Username))
                return _responseHandler.Conflict<User>("User already exists.");

            var hashedPassword = _passwordHasher.Hash(model.Password);
            var user = new User
            {
                Username = model.Username,
                Email = model.Email,
                PasswordHash = hashedPassword
            };

            await _userRepository.AddAsync(user);

            return _responseHandler.Created(user);
        }

        public string GenerateJwtToken(User user)
        {
            var jwtOption = _configuration.GetSection("JWT").Get<JwtOptions>();
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(jwtOption.SigningKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Email, user.Email)
                }),
                Expires = DateTime.UtcNow.AddHours(jwtOption.LifeTime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}


