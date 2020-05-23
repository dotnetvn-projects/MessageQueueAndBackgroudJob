using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QueueEngine.Interfaces;
using QueueEngine.Models.QueueData;
using WebSample.Models;

namespace WebSample.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private IQueueService<OrderQueue> _queueService;
        public HomeController(ILogger<HomeController> logger,
            IQueueService<OrderQueue> queueService)
        {
            _queueService = queueService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Purchase()
        {
            _queueService.PushQueue(new OrderQueue
            {
                CustomerName = "Customer_" + Guid.NewGuid(),
                OrderId = Guid.NewGuid(),
                ProductId = Guid.NewGuid()
            });

            return RedirectToAction("Index");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
