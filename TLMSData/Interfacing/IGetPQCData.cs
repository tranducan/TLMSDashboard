using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Models;

namespace TLMSData.Interfacing
{
    public interface IGetPQCData
    {
        Task<List<ProductionLine>> GetProductionLines(DateTime StartTime, DateTime EndTime);

        Task<List<ProductionLine>> GetProductionLinesFilterLine(string LineFilter, DateTime StartTime, DateTime EndTime);

        Task<List<ProductionRealtime>> GetProductionRealtimes(string Line, DateTime StartTime, DateTime EndTime);

    }
}
