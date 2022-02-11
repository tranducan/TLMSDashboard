using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using TLMSDashboard.Models;
using TLMSData.Processing;
using TLMSData.Interfacing;
using EFTechlink.Model;


namespace TLMSDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public static EFTechlink.EFCore.TLMSDataContext context = new EFTechlink.EFCore.TLMSDataContext();

        private IMqcDataInteracting mqcDataInteracting = new MqcDataInteracting(context);

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            DateTime dateTimeStart = new DateTime(2019, 12, 3, 0, 0, 0);
            DateTime dateTimeEnd= new DateTime(2012, 12, 30, 0, 0, 0);
            var mqcDataSummary = mqcDataInteracting.GetMqcDataSummary(dateTimeStart, dateTimeEnd).Result;

            return View(mqcDataSummary);
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
    }
}
