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
    public class AnalysisController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
