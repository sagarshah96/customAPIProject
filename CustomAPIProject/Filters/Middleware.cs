using CustomAPIProject.ApplicationContext;
using CustomAPIProject.Repository;
using CustomAPIProject.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomAPIProject.Filters
{
    public class Middleware
    {
        private readonly RequestDelegate _next;
        private readonly AppSettings _appSettings;
        private readonly ILogger logger;

        public Middleware(RequestDelegate next, IOptions<AppSettings> appSettings, ILogger<UnhandledExceptionMiddleware> logger)
        {
            _next = next;
            _appSettings = appSettings.Value;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context, _ILoginService _Login)
        {

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            string controllerName = (string)context.Request.RouteValues["Controller"];
            bool isValid = false;
            if (token != null)
            {
                isValid = ValidateToken(context, token);

                bool isDbValid = _Login.TokenValidateInDB(token);

                if (!isValid && !isDbValid)
                {
                    await TokenError(context, "Invalid Token.!!");
                    return;

                }
                if (!isDbValid)
                {
                    await TokenError(context, "Invalid Token.!!");
                    return;
                }
                await _next(context);
            }
            else
            {
                if (controllerName != "Login")
                {
                    await TokenError(context, "Please Enter Token.!!");
                    return;
                }
                await _next(context);
            }
        }

        private bool ValidateToken(HttpContext context, string token)
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
                int RoleId = int.Parse(jwtToken.Claims.First(x => x.Type == "RoleId").Value);
                string RoleName = Convert.ToString(jwtToken.Claims.First(x => x.Type == "RoleName").Value);

                context.Items["CustomerId"] = CustomerId;
                context.Items["RoleName"] = RoleName;
                context.Items["RoleId"] = RoleId;
            }
            catch
            {
                return false;
            }
            return true;
        }

        private async Task TokenError(HttpContext context,string msg)
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsJsonAsync(
                new { msg = msg }
                );
            logger.LogError(msg + $"Request {context.Request?.Method}: {context.Request?.Path.Value} failed");
        }
    }
}

