using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Models;
using EFTechlink.EFCore;

namespace TLMSData.Interfacing
{
    public interface ISetDailyTarget
    {
        Task InsertDailyTarget(DailyPerformanceGoal dailyPerformanceGoal);

        Task UpdateDailyTarget(DailyPerformanceGoal dailyPerformanceGoal);

        Task DeleteDailyTarget(DailyPerformanceGoal dailyPerformanceGoal);

        Task <DailyPerformanceGoal> SelectDailyTargetbyProduct(string productName);
        
    }
}
