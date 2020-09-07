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
using Microsoft.EntityFrameworkCore;

namespace BillingAPI.Controllers
{
    //[EnableCors("AllowOrigin")]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private IConfiguration _config;
        private readonly DBBillingContext _context;
        private int _userID = 0;
        private bool IsAdmin = false;

        public OrderController(IConfiguration config, DBBillingContext context)
        {
            _config = config;
            _context = context;
        }
        [HttpGet]
        public IActionResult Get()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Orders> orders = new List<Orders>();
            if (identity != null)
            {
                _userID = Convert.ToInt32(identity.FindFirst("UserID").Value);
                IsAdmin = Convert.ToBoolean(identity.FindFirst("IsAdmin").Value);
            }
            var response = _context.Orders.Where(x => x.UserId == (IsAdmin ? x.UserId : _userID)).Select(p => new
            {
                p.OrderDate,
                p.OrderId,
                p.Customer.CustomerName,
                p.Customer.Address,
                p.Customer.Phone

            }).OrderByDescending(x => x.OrderId);



            var res = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            return Ok(res);
        }
        [HttpGet("report/{From}/{To}")]
        public IActionResult GetOrderReport(DateTime From, DateTime To)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            List<Orders> orders = new List<Orders>();
            var response = _context.Orders.Include(o => o.OrderDetails).Where(x => x.OrderDate <= To && x.OrderDate >= From).Select(p => new
            {
                p.OrderDate,
                p.OrderId,
                EmployeeName = p.User.FirstName + " " + p.User.LastName,
                CustomerName = string.IsNullOrEmpty(p.Customer.CustomerName) ? "NA" : p.Customer.CustomerName,
                Total = p.OrderDetails.Sum(x => x.Rate) > 0 ? p.OrderDetails.Sum(x => x.Rate) : 0

            }).OrderByDescending(x => x.EmployeeName);

            var res = Newtonsoft.Json.JsonConvert.SerializeObject(response);
            return Ok(res);
        }
        [HttpGet("Details")]
        public IActionResult GetOrderDetails(int OrderID)
        {
            return Ok(_context.OrderDetails.Where(x => x.OrderId == OrderID).ToList());
        }
        [HttpPost]
        public IActionResult Post(Orders request)
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                //IEnumerable<Claim> claims = identity.Claims;
                _userID = Convert.ToInt32(identity.FindFirst("UserID").Value);

            }
            request.UserId = _userID;
            _context.Orders.Add(request);
            return Ok(_context.SaveChanges());
        }
    }
}