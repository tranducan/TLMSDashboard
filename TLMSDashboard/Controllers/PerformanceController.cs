using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TLMSDashboard.Models;
using TLMSData.Processing;
using TLMSData.Interfacing;
using EFTechlink.EFCore;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using TLMSData.Models;

namespace TLMSDashboard.Controllers
{
    public class PerformanceController : Controller
    {
        private readonly ILogger<PerformanceController> _logger;
        private readonly IConfiguration _config;
        private ISetDailyTarget setDailyTarget;
        private readonly TLMSDataContext dataContext;

        public PerformanceController(
            ILogger<PerformanceController> logger,
            IConfiguration configuration,
            TLMSDataContext context)
        {
            _logger = logger;
            _config = configuration;
            dataContext = context;
            setDailyTarget = new SetDailyTarget(dataContext);

        }

        [HttpGet]
        public async Task<IActionResult> SetTarget(ProductTarget target)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SetTarget(ProductTarget target, string returnUrl = null)
        {
            var dailyPerformancegoal = new DailyPerformanceGoal()
            {
                Model = target.ProductName,
                Process = "PQC",
                Site = "TL01",
                Line = "L01",
                OutputTarget = target.OutputTarget,
                ReworkTarget = target.ReworkTarget,
                NotGoodTarget = target.NotGoodTarget,
                StartDate = target.StartDate,
                EndDate = target.EndDate,
            };

            await setDailyTarget.InsertDailyTarget(dailyPerformancegoal);

            return View();
        }
    }
}
