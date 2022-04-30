using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Interfacing;
using TLMSData.Models;
using EFTechlink.EFCore;
using Microsoft.Data.SqlClient;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace TLMSData.Processing
{
    public class GetPQCDataAnalysis : IGetPQCDataAnalysis
    {
        private readonly string connectionString;
        private readonly TLMSDataContext tLMSDataContext;

        public GetPQCDataAnalysis(
            TLMSDataContext dataContext)
        {
            tLMSDataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            connectionString = tLMSDataContext.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<ProductionAnalysis>> GetProductionAnalyses(DateTime startTime, DateTime endTime)
        {
            var listReturn = new List<ProductionAnalysis>();
            string spName = "[ProcessHistory].[GetPQCMesDataAnalyst]";
            using (var conn = new SqlConnection(connectionString))
            using (var command = new SqlCommand(spName, conn))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectStart", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@InspectEnd", endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while(rdr.Read())
                    {
                        var productionAnalysis = new ProductionAnalysis()
                        {
                            Date = (DateTime)rdr["Date"],
                            Shift = (string)rdr["WorkingShift"],
                            InspectStart = (DateTime)rdr["StartTime"],
                            InspectEnd = (DateTime)rdr["EndTime"],
                            Line = rdr["Line"].ToString(),
                            Product = rdr["Product"].ToString(),
                            OPTarget = (decimal) rdr["OPTarget"],
                            NGTarget = (decimal)rdr["NGTarget"],
                            RWTarget = (decimal)rdr["RWTarget"],
                            Output = (decimal)rdr["OutputActual"],
                        }; 
                    }
                }

                return listReturn;
            }
        }

        public async Task<List<ProductionAnalysis>> GetProductionAnalysesByLine(string line, DateTime startTime, DateTime endTime)
        {
            throw new NotImplementedException();
        }
    }
}
