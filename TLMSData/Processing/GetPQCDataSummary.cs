using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Interfacing;
using TLMSData.Models;

namespace TLMSData.Processing
{
    public class GetPQCDataSummary : IGetPQCDataSummary
    {
        public Task<List<ProductionSummary>> GetProductionSummary(DateTime StartTime, DateTime EndTime)
        {
            throw new NotImplementedException();
        }

        public Task<List<ProductionSummary>> GetProductionSummarybyLine(string line, DateTime StartTime, DateTime EndTime)
        {
            throw new NotImplementedException();
        }
    }
}
