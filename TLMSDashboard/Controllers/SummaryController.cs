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
    public class SummaryController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration _config;
        private IGetPQCDataSummary getPQCDataSummary;

        public SummaryController(
            ILogger<HomeController> logger,
            IConfiguration configuration,
            TLMSDataContext context)
        {
            _logger = logger;
            _config = configuration;
            getPQCDataSummary = new GetPQCDataSummary(context);
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(DateTime StartDate, DateTime EndDate, string line)
        {
            if (line is null)
            {
                throw new ArgumentNullException("input of line cannot be null");
            }

            if (StartDate == DateTime.MinValue)
            {
                throw new InvalidOperationException("Please input correct Start Date");
            }

            if (EndDate == DateTime.MinValue)
            {
                throw new InvalidOperationException("Please input correct End Date");
            }

            if (line != "ALL")
            {
                var result = getPQCDataSummary.GetProductionSummarybyLinebyShiftDate(line, StartDate, EndDate)?.Result;

                return View(result);
            }
            else
            {
                var result = getPQCDataSummary.GetProductionSummarybyShiftDate(StartDate, EndDate)?.Result;

                return View(result);
            }
        }
    }
}
