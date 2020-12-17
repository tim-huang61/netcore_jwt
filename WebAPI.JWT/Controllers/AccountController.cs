using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WebAPI.JWT.Models;

namespace WebAPI.JWT.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AccountController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public ActionResult<JwtToken> Token([FromBody] LoginViewModel loginViewModel)
        {
            var expires = DateTime.Now.AddMinutes(20);
            var key = _configuration["JWT_Setting:Key"];
            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, loginViewModel.Account),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new(ClaimTypes.Role, "Admin"),
            };

            var claimsIdentity = new ClaimsIdentity(claims);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
            {
                Subject = claimsIdentity,
                Issuer = "Tim",
                SigningCredentials = credentials,
                Expires = expires,
            });

            var token = tokenHandler.WriteToken(securityToken);

            return Ok(new JwtToken
            {
                Token = token,
                ExpiredTime = expires,
            });
        }
    }
}