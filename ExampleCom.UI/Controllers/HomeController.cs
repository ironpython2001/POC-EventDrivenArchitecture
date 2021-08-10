using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ExampleCom.UI.Models;
using ExampleCom.EventHub.POCOs;
using ExampleCom.EventHub.Producer;
using Microsoft.Extensions.Configuration;

namespace ExampleCom.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;

        public HomeController(ILogger<HomeController> logger,IConfiguration config)
        {
            _logger = logger;
            _config = config;
                
        }

        public IActionResult Index()
        {
            var model = new Order()
            {
                OrderId = 0,
                CustomerId = 0,
                Products = new List<Product>
                {
                    new Product
                    {
                        ProductId = 0,
                        Quantity=0
                    }
                }
            };
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(Order order)
        {
            var bootstrapServers = this._config["BootstrapServers"].ToString();
            var username = this._config["un"].ToString();
            var password = this._config["pwd"].ToString();
            var topic = "8rvw9okw-order-received";
            var msg = new OrderMessage();
            var result = await msg.Create(bootstrapServers, username, password, topic,order);
            return View(order);
        }

    }
}
