using EFTechlink.EFCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [HttpPost("[action]")]
        public IActionResult Insert([FromBody]CrudViewModel<DailyPerformanceGoal> payload)
        {
            DailyPerformanceGoal goal = payload.value;
            dataContext.DailyPerformanceGoals.Add(goal);
            dataContext.SaveChanges();

            return Ok(goal);
        }

        [HttpPost("[action]")]
        public IActionResult Update([FromBody]CrudViewModel<DailyPerformanceGoal> payload)
        {
            DailyPerformanceGoal goal = payload.value;
            dataContext.DailyPerformanceGoals.Update(goal);
            dataContext.SaveChanges();

            return Ok(goal);
        }

        [HttpPost("[action]")]
        public IActionResult Remove([FromBody] CrudViewModel<DailyPerformanceGoal> payload)
        {
            DailyPerformanceGoal goal = dataContext.DailyPerformanceGoals
                .Where(x => x.DailyPerformanceGoaId == (int)payload.key)
                .FirstOrDefault();

            dataContext.DailyPerformanceGoals.Remove(goal);
            dataContext.SaveChanges();

            return Ok(goal);
        }
    }
}
