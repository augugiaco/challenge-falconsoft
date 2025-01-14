
using FalconSoftChallenge.Business.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FalconSoftChallenge.API.Security
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;

        public AuthService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<string> LoginAndGenerateAccessToken(UserLoginModel userLoginModel)
        {
            if (userLoginModel == null) throw new ArgumentNullException("Missing login parameters");

            var user = await _userService.GetByEmailAndPassword(userLoginModel.Email, userLoginModel.Password);

            if (user == null) throw new UnauthorizedAccessException();

            return GenerateJwtToken(user.Id, user.Email);
        }

        private string GenerateJwtToken(Guid id, string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Email, email),
                new Claim(JwtRegisteredClaimNames.Prn, id.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("bbbbbbbbbbbbbbbbbbccccccccccccccddddddddddddeeeeeeee124567789"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "falconsoft.cl",
                audience: "falconsoft.cl",
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
