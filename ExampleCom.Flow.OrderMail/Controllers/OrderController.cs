using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleCom.EventHub.POCOs;
using ExampleCom.EventHub.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExampleCom.Flow.OrderMail.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {

        private readonly ILogger<OrderController> _logger;
        private readonly IConfiguration _config;

        public OrderController(ILogger<OrderController> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        [HttpPost("ordermail")]
        public async Task<IActionResult> OrderMail(Order order)
        {
            Console.WriteLine("Order Emailed");
            return await Task.FromResult(Ok());
        }
    }
}
