using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Models;

namespace TLMSData.Interfacing
{
    public interface IGetPQCDataAnalysis
    {
        Task<List<ProductionAnalysis>> GetProductionAnalyses(DateTime startTime, DateTime endTime);

        Task<List<ProductionAnalysis>> GetProductionAnalysesByLine(string line, DateTime startTime, DateTime endTime);
    }
}
