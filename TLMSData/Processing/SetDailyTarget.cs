using EFTechlink.EFCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Interfacing;
using System.Linq;

namespace TLMSData.Processing
{
    public class SetDailyTarget : ISetDailyTarget
    {
        private readonly TLMSDataContext dataContext;

        public SetDailyTarget(
            TLMSDataContext context)
        {          
            this.dataContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task DeleteDailyTarget(DailyPerformanceGoal dailyPerformanceGoal)
        {
           if (dailyPerformanceGoal is null)
                throw new ArgumentNullException(nameof(dailyPerformanceGoal));

            dataContext.DailyPerformanceGoals.Remove(dailyPerformanceGoal);
            dataContext.SaveChangesAsync();
        }

        public async Task InsertDailyTarget(DailyPerformanceGoal dailyPerformanceGoal)
        {
            if (dailyPerformanceGoal is null)
                throw new ArgumentNullException(nameof(dailyPerformanceGoal));

            try
            {
                using (var tx = await dataContext.Database.BeginTransactionAsync(default))
                {
                    dataContext.DailyPerformanceGoals.Add(dailyPerformanceGoal);

                    await dataContext.SaveChangesAsync(default).ConfigureAwait(false);

                    await tx.CommitAsync(default).ConfigureAwait(false);
                }

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<DailyPerformanceGoal> SelectDailyTargetbyProduct(string productName)
        {
            if (productName is null)
                throw new ArgumentNullException(nameof(productName));

            var searchResult = dataContext.DailyPerformanceGoals.Where(d => d.Model == productName)
                                                                 .Where(d => d.StartDate < DateTime.Now)
                                                                 .Where(d => d.EndDate == null || d.EndDate > DateTime.Now)
                                                                .FirstOrDefault();
            return searchResult;
        }

        public async Task UpdateDailyTarget(DailyPerformanceGoal dailyPerformanceGoal)
        {
            if (dailyPerformanceGoal is null)
                throw new ArgumentNullException(nameof(dailyPerformanceGoal));

            var searchResult = dataContext.DailyPerformanceGoals.Where(d => d.Model == dailyPerformanceGoal.Model)
                                                                .FirstOrDefault();
            if (searchResult != null)
            {
                var dataUpdated = new DailyPerformanceGoal()
                {
                    DailyPerformanceGoaId = searchResult.DailyPerformanceGoaId,
                    Model = searchResult.Model,
                    Line = searchResult.Line == null ? String.Empty : searchResult.Line,
                    Process = searchResult.Process == null ? String.Empty : searchResult.Process,
                    Site = searchResult.Site == null ? String.Empty : searchResult.Site,
                    StartDate = searchResult.StartDate,
                    EndDate = searchResult.EndDate,
                    OutputTarget = searchResult.OutputTarget,
                    NotGoodTarget = searchResult.NotGoodTarget,
                    ReworkTarget = searchResult.ReworkTarget,
                };

                dataContext.Update<DailyPerformanceGoal>(dataUpdated);
            }
        }
    }
}
