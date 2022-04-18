using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAPIProject.Filters
{
    public class CustomMiddleware : IMiddleware
    {
        private readonly AppSettings _appSettings;

        public CustomMiddleware(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
            {
                bool IsValid = attachUserToContext(context, token);
                if (!IsValid)
                    await context.Response.WriteAsync("Invalid Token");
            }

            await next(context);
        }

        private bool attachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                int CustomerId = int.Parse(jwtToken.Claims.First(x => x.Type == "CustomerId").Value);
                string Role = jwtToken.Claims.First(x => x.Type == "Role").Value;

                context.Items["CustomerId"] = CustomerId;
                context.Items["Role"] = Role;
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
