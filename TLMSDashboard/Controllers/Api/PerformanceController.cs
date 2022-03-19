using EFTechlink.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TLMSDashboard.Models;

namespace TLMSDashboard.Controllers.Api
{
    [Produces("application/json")]
    [Route("api/Performance")]
    public class PerformanceController : Controller
    {
        private readonly TLMSDataContext dataContext;

        public PerformanceController(TLMSDataContext context)
        {
            dataContext = context;
        }

        // GET: api/Target
        [HttpGet]
        public async Task<IActionResult> GetTarget()
        {
            List<DailyPerformanceGoal> performanceGoals = await dataContext.DailyPerformanceGoals.ToListAsync();
            int count = performanceGoals.Count();

            return Ok(new {performanceGoals, count});
        }

        [HttpPost]
        public IActionResult Insert([FromBody]CrudViewModel<DailyPerformanceGoal> payload)
        {
            if (payload.value is null)
            {
                throw new ArgumentNullException("payload received a null argument!");
            }

            DailyPerformanceGoal goal = payload.value;
            dataContext.DailyPerformanceGoals.Add(goal);
            dataContext.SaveChanges();

            return Ok(goal);
        }

        [HttpPut]
        public IActionResult Update([FromBody]CrudViewModel<DailyPerformanceGoal> payload)
        {
            if (payload.value is null)
            {
                throw new ArgumentNullException("payload received a null argument!");
            }

            DailyPerformanceGoal goal = payload.value;
            dataContext.DailyPerformanceGoals.Update(goal);
            dataContext.SaveChanges();

            return Ok(goal);
        }

        [HttpDelete]
        public IActionResult Remove([FromBody] CrudViewModel<DailyPerformanceGoal> payload)
        {
            if (payload.value is null)
            {
                throw new ArgumentNullException("payload received a null argument!");
            }

            DailyPerformanceGoal goal = dataContext.DailyPerformanceGoals
                .Where(x => x.Model == payload.value.Model)
                .FirstOrDefault();

            dataContext.DailyPerformanceGoals.Remove(goal);
            dataContext.SaveChanges();

            return Ok(goal);
        }
    }
}
