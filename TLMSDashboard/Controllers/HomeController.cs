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
using EFTechlink.EFCore;
using Microsoft.Extensions.Configuration;

namespace TLMSDashboard.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private IMqcDataInteracting mqcDataInteracting;
        private IGetPQCData getPQCData;
        private string ConnectionString;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            TLMSDataContext context)
        {
            _logger = logger;
            _config = configuration;
            mqcDataInteracting = new MqcDataInteracting(context);
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            getPQCData = new GetPQCData(ConnectionString);
        }

        public IActionResult Index()
        {
            DateTime dateTimeStart = new DateTime(2019, 12, 3, 0, 0, 0);
            DateTime dateTimeEnd = new DateTime(2012, 12, 30, 0, 0, 0);
            var mqcDataSummary = mqcDataInteracting.GetMqcDataSummary(dateTimeStart, dateTimeEnd).Result;

            return View(mqcDataSummary);
        }

        public IActionResult PqcIndex()
        {
            DateTime dateTimeStart = new DateTime(2019, 12, 3, 0, 0, 0);
            DateTime dateTimeEnd = DateTime.Now;
            var pqcDataSummary = mqcDataInteracting.GetPqcDataSummary(dateTimeStart, dateTimeEnd).Result;

            return View(pqcDataSummary);
        }

        public IActionResult PqcDataIndex()
        {
            DateTime dateTimeStart = new DateTime(2019, 12, 3, 0, 0, 0);
            DateTime dateTimeEnd = DateTime.Now;
            var pqcDataSummary = mqcDataInteracting.GetPqcData(dateTimeStart, dateTimeEnd).Result;

            return View(pqcDataSummary);
        }

        public IActionResult Models()
        {
            DateTime dateTimeStart = new DateTime(2019, 12, 3, 0, 0, 0);
            DateTime dateTimeEnd = DateTime.Now;
            var result = getPQCData.GetProductionLines(dateTimeStart, dateTimeEnd).Result;

            return View(result);
        }

        public IActionResult ProductionInformation()
        {
            DateTime dateTimeStart = new DateTime(2019, 12, 3, 0, 0, 0);
            DateTime dateTimeEnd = DateTime.Now;
            var result = getPQCData.GetProductionInformation(dateTimeStart, dateTimeEnd).Result;

            return View(result);
        }

        public IActionResult Activities()
        {
            DateTime dateTimeStart = new DateTime(2019, 12, 3, 0, 0, 0);
            DateTime dateTimeEnd = DateTime.Now;
            string line = "L01";
            var result = getPQCData.GetProductionRealtimes(line, dateTimeStart, dateTimeEnd);

            return View();
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
