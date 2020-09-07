using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BillingAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace BillingAPI.Controllers
{
    //[EnableCors("AllowOrigin")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private IConfiguration _config;
        private readonly DBBillingContext _context;
        private int _userID = 0;

        public CustomerController(IConfiguration config, DBBillingContext context)
        {
            _config = config;
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                _userID = Convert.ToInt32(identity.FindFirst("UserID").Value);
            }
            return Ok(_context.Customer.Where(x => x.UserId == _userID).OrderByDescending(x=>x.CreatedDate).ToList());
        }
        [HttpPost]
        public IActionResult Post(Customer request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                _userID = Convert.ToInt32(identity.FindFirst("UserID").Value);
            }
            request.UserId = _userID;
            request.CreatedDate = DateTime.Now;
            _context.Customer.Add(request);
            var rs = _context.SaveChanges();
            return Ok(rs);
        }
    }
}