using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using visionet_webapi.Common.Command;
using visionet_webapi.Common.Const;
using visionet_webapi.Common.Dto;
using visionet_webapi.Exceptions;
using visionet_webapi.Repository;

namespace visionet_webapi.Services.Impl
{
    public class AuthServiceImpl : IAuthService
    {
        private readonly DataContext db;

        public AuthServiceImpl(DataContext db)
        {
            this.db = db;
        }

        public LoginDto Login(AuthCommand command)
        {
            LoginDto response = new LoginDto();

            var user = db.User.FirstOrDefault(x => x.Username == command.Username && x.Password == command.Password);
            if (user == null)
            {
                throw new BadRequestException("username or password wrong");
            }

            var token = GenerateAccessToken(user.Id, user.RoleId.GetValueOrDefault());
            response = new LoginDto()
            {
                Token = token,
                RoleId = user.RoleId.GetValueOrDefault(),
                UserId = user.Id,
                Username = user.Username
            };

            return response;
        }

        private string GenerateAccessToken(int userId, int roleId)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConst.Secret));
            var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = JwtConst.Issuer,
                Audience = JwtConst.Audience,
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtConst.UserId, userId.ToString()),
                    new Claim(JwtConst.RoleId, roleId.ToString())
                }),
                Expires = DateTime.Now.AddMinutes(JwtConst.ExpiryMinutes),
                SigningCredentials = credential
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
