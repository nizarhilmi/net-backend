using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using visionet_webapi.Common.Const;

namespace visionet_webapi.Middleware
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate next;

        public JwtMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            if (token != null)
            {
                var (userId, roleId) = ValidateAccessToken(token);
                context.Items["UserId"] = userId;
                context.Items["RoleId"] = roleId;
            }

            await next(context);
        }

        private (string? userId, string? roleId) ValidateAccessToken(string accessToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtConst.Secret));

                tokenHandler.ValidateToken(accessToken, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = key,
                    ValidIssuer = JwtConst.Issuer,
                    ValidAudience = JwtConst.Audience,
                    ClockSkew = TimeSpan.Zero
                }, out var validatedToken);

                var jwtToken = validatedToken as JwtSecurityToken;

                var userId = jwtToken?.Claims.FirstOrDefault(x => x.Type == JwtConst.UserId)?.Value;
                var roleId = jwtToken?.Claims.FirstOrDefault(x => x.Type == JwtConst.RoleId)?.Value;
                return (userId, roleId);
            }
            catch
            {
                return (null, null);
            }
        }
    }
}
