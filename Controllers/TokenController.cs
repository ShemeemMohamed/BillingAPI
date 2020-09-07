using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BillingAPI.Models;
using BillingAPI.RequestModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BillingAPI.Controllers
{
    //[EnableCors("AllowOrigin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private IConfiguration _config;
        private readonly DBBillingContext _context;

        public TokenController(IConfiguration config, DBBillingContext context)
        {
            _config = config;
            _context = context;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] Login request)
        {
            Response response = new Response();
            var user = AuthenticateUser(request);

            if (user != null)
            {
                response.Code = 0;
                response.Message = "Success";
                var tokenString = GenerateJSONWebToken(user);
                response.Data =new { token = tokenString,
                     user = user };
            }
            else
            {
                response.Code = 2;
                response.Message = "Invalid Credentials";
            }

            return Ok(response);
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new Claim[]
            {
                    new Claim("UserID", userInfo.Id.ToString()),
                    new Claim("IsAdmin", Convert.ToString(userInfo.IsAdmin)),
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],claims: claims,              
              expires: DateTime.Now.AddMinutes(120),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private User AuthenticateUser(Login request)
        {
            return _context.User.FirstOrDefault(x => x.Username == request.Username && x.Password == request.Password);
        }
    }
}