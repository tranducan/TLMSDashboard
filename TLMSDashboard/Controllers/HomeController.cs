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
        private DateTime setTimeStart = DateTime.MinValue;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            TLMSDataContext context)
        {
            _logger = logger;
            _config = configuration;
            mqcDataInteracting = new MqcDataInteracting(context);
            ConnectionString = configuration.GetConnectionString("DefaultConnection");
            getPQCData = new GetPQCData( context);
            if (Parameters.SetStaticValue.isDevEnvironment)
            {
                DateTime setStartDate = configuration.GetValue<DateTime>("StartTime:Date");
                int setHour = configuration.GetValue<int>("StartTime:Hour");
                int setMinute = configuration.GetValue<int>("StartTime:Minute");
                setTimeStart = setStartDate.AddHours(setHour).AddMinutes(setMinute);
            }
            else
            {
                DateTime setStartDate = DateTime.Now.Date;
                int setHour = configuration.GetValue<int>("StartTime:Hour");
                int setMinute = configuration.GetValue<int>("StartTime:Minute");
                setTimeStart = setStartDate.AddHours(setHour).AddMinutes(setMinute);
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult PqcIndex()
        {
            DateTime dateTimeStart = setTimeStart;
            DateTime dateTimeEnd = DateTime.Now;
            var pqcDataSummary = mqcDataInteracting.GetPqcDataSummary(dateTimeStart, dateTimeEnd)?.Result;

            return View(pqcDataSummary);
        }

        public IActionResult PqcDataIndex()
        {
            DateTime dateTimeStart =  setTimeStart;
            DateTime dateTimeEnd = DateTime.Now;
            var pqcDataSummary = mqcDataInteracting.GetPqcData(dateTimeStart, dateTimeEnd)?.Result;

            return View(pqcDataSummary);
        }

        public IActionResult Models()
        {
            DateTime dateTimeStart = setTimeStart;
            DateTime dateTimeEnd = DateTime.Now;
            var result = getPQCData.GetProductionLines(dateTimeStart, dateTimeEnd)?.Result;

            return View(result);
        }

        public IActionResult PQCProduction()
        {
            DateTime dateTimeStart = setTimeStart;
            DateTime dateTimeEnd = DateTime.Now;
            var result = getPQCData.GetProductionInformation(dateTimeStart, dateTimeEnd)?.Result;

            return View(result);
        }

        public IActionResult Activities()
        {
            DateTime dateTimeStart = setTimeStart;
            DateTime dateTimeEnd = DateTime.Now;
            string line = "L01";
            var result = getPQCData.GetProductionRealtimes(line, dateTimeStart, dateTimeEnd)?.Result;

            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Settings()
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
