﻿using CustomAPIProject.ApplicationContext;
using CustomAPIProject.Filters;
using CustomAPIProject.Models;
using CustomAPIProject.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CustomAPIProject.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly _IRepository<Login> _LoginRepo;
        private readonly _IRepository<Customer> _CustomerRepo;
        private readonly AppSettings _appSettings;

        public LoginController(_IRepository<Login> lrepository, IOptions<AppSettings> appSettings, _IRepository<Customer> crepository)
        {
            _LoginRepo = lrepository;
            _appSettings = appSettings.Value;
            _CustomerRepo = crepository;
        }

        //[Validate]
        [MapToApiVersion("1.0")]
        [HttpPost]
        public IActionResult Login(LoginModel loginModel)
        {
            //if (!ModelState.IsValid)
            //{
            //    return BadRequest(ModelState.SelectMany(x => x.Value.Errors));
            //}
            //string str = Common.Decrypt(loginModel.Password);
            //string str1 = Common.Encrypt(loginModel.Password);
            var objlogin = _LoginRepo.GetAll().FirstOrDefault(x => x.Email == loginModel.Email && x.Password == Common.Encrypt(loginModel.Password));
            var token = "";
            if (objlogin != null)
            {
                bool isActive = _CustomerRepo.GetByID(objlogin.CustomerId).IsActive;
                if (isActive)
                {
                    token = generateJwtToken(objlogin);
                    objlogin.Token = token;
                    _LoginRepo.Update(objlogin);
                    return Ok(token);
                }
                else
                    return Ok("Your account has been deactivated. Please contact to admin.");
            }
            return NotFound("Invalid User Name and Password.");

        }

        [MapToApiVersion("1.0")]
        [Authorize(Roles.Admin, Roles.Customer)]
        [HttpPost]
        public IActionResult Logout()
        {
            int CustomerId = (int)HttpContext.Items["CustomerId"];
            if (CustomerId > 0)
            {
                var customer = _LoginRepo.GetByID(CustomerId);
                customer.Token = "";
                _LoginRepo.Update(customer);
                return Ok("Logout Successfully.!!");
            }
            return NotFound();
        }

        private string generateJwtToken(Login obj)
        {
            // generate token that is valid for 30 minutes
            var tokenHandler = new JwtSecurityTokenHandler();
            //Secret Key must be >= 16 char
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var role = (Roles)obj.RoleId;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("LoginId", obj.LoginId.ToString()),
                    new Claim("Email", obj.Email.ToString()),
                    new Claim("CustomerId", obj.CustomerId.ToString()),
                    new Claim("RoleName", role.ToString()),
                    new Claim("RoleId", obj.RoleId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
