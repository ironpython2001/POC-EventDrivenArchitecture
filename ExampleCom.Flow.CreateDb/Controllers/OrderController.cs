using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExampleCom.EventHub.POCOs;
using ExampleCom.EventHub.Producer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExampleCom.Flow.CreateDb.Controllers
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

        [HttpPost("createorder")]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            Console.WriteLine("Order Created");

            var bootstrapServers = this._config["BootstrapServers"].ToString();
            var username = this._config["un"].ToString();
            var password = this._config["pwd"].ToString();
            var topic = "8rvw9okw-order-created";
            var msg = new OrderMessage();
            var result = await msg.Create(bootstrapServers, username, password, topic, order);
            return await Task.FromResult(Ok());
        }

    }
}
