using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Models;

namespace TLMSData.Interfacing
{
    public interface IGetPQCDataSummary
    {
        Task<List<ProductionSummary>> GetProductionSummary(DateTime StartTime, DateTime EndTime);

        Task<List<ProductionSummary>> GetProductionSummarybyLine(string line, DateTime StartTime, DateTime EndTime);

        Task<List<ProductionSummary>> GetProductionSummarybyDate(DateTime StartTime, DateTime EndTime);

        Task<List<ProductionSummary>> GetProductionSummarybyLinebyDate(string line, DateTime StartTime, DateTime EndTime);
    }
}
