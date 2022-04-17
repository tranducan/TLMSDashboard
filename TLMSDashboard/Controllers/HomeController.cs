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
        private IGetPQCData getPQCData;
        private DateTime setTimeStart = DateTime.MinValue;
        private DateTime setTimeEnd= DateTime.MinValue;

        public HomeController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            TLMSDataContext context)
        {
            _logger = logger;
            _config = configuration;
            getPQCData = new GetPQCData(context);
            if (Parameters.SetStaticValue.isDevEnvironment)
            {
                DateTime setStartDate = configuration.GetValue<DateTime>("2019-01-01");
                int setHour = configuration.GetValue<int>("StartShift:Hour");
                int setMinute = configuration.GetValue<int>("StartShift:Minute");
                setTimeStart = setStartDate.AddHours(setHour).AddMinutes(setMinute);
                setTimeEnd = DateTime.Now;
            }
            else
            {
                int setStartHour = configuration.GetValue<int>("StartShift:Hour");
                int setEndHour = configuration.GetValue<int>("EndShift:Hour");
                if (DateTime.Now.Hour >= setStartHour && DateTime.Now.Hour < setEndHour)
                {
                    setTimeStart = DateTime.Now.Date.AddHours(setStartHour);
                    setTimeEnd = DateTime.Now.Date.AddHours(setEndHour);
                }
                else
                {
                    if (DateTime.Now.Hour >= setEndHour)
                    {
                        setTimeStart = DateTime.Now.Date.AddHours(setEndHour);
                        setTimeEnd = DateTime.Now.Date.AddDays(1).AddHours(setStartHour);
                    }
                    else
                    {
                        setTimeStart = DateTime.Now.Date.AddDays(-1).AddHours(setEndHour);
                        setTimeEnd = DateTime.Now.Date.AddHours(setStartHour);
                    }
                }
            }
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Models()
        {
            DateTime dateTimeStart = setTimeStart;
            DateTime dateTimeEnd = setTimeEnd;
            var result = getPQCData.GetProductionLines(dateTimeStart, dateTimeEnd)?.Result;

            return View(result);
        }

        public IActionResult PQCMaster()
        {
            DateTime dateTimeStart = setTimeStart;
            DateTime dateTimeEnd = setTimeEnd;

            var result = getPQCData.GetProductionInformation(dateTimeStart, dateTimeEnd)?.Result;

            return View(result);
        }

        public IActionResult PQCProduction(string line)
        {
            DateTime dateTimeStart = setTimeStart;
            DateTime dateTimeEnd = setTimeEnd;
            if (line is null)
            {
                return View(new TLMSData.Models.ProductionInformation());
            }

            if (line == "ALL")
            {
                var result = getPQCData.GetProductionInformation(dateTimeStart, dateTimeEnd)?.Result;

                return View(result);
            }
            else
            {
                var result = getPQCData.GetProductionInformationbyLine(line, dateTimeStart, dateTimeEnd)?.Result;

                return View(result);
            }
        }

        public IActionResult Activities(string line)
        {
            if (line is null)
            {
                throw new ArgumentNullException("input of line cannot be null");
            }

            DateTime dateTimeStart = setTimeStart;
            DateTime dateTimeEnd = setTimeEnd;
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

        public IActionResult LineQuantity()
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
