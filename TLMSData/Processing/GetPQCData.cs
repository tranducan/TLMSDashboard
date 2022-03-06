using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Interfacing;
using TLMSData.Models;
using EFTechlink.EFCore;
using Microsoft.Data.SqlClient;

namespace TLMSData.Processing
{
    public class GetPQCData : IGetPQCData
    {

        private readonly string ConnectionString;

        public GetPQCData(
        string connectionString)
        {
            this.ConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<List<ProductionLine>> GetProductionLines(DateTime StartTime, DateTime EndTime)
        {
            var listReturn = new List<ProductionLine>();
            string SPName = "[ProcessHistory].[GetPQCMesData]";
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(SPName, conn))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectStart", StartTime.ToString());
                command.Parameters.AddWithValue("@InspectEnd", EndTime.ToString());
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var productLine = new ProductionLine()
                        {
                            Line = (string)rdr["Line"],
                            Product = (string)rdr["Product"],
                            InspectStart = (DateTime)rdr["StartTime"],
                            InspectEnd = (DateTime)rdr["EndTime"],
                            Output = (decimal)rdr["OPQty"],
                            NotGood = (decimal)rdr["NGQty"],
                            Rework = (decimal)rdr["RWQty"]
                        };
                        listReturn.Add(productLine);
                    }
                }
            }

            return listReturn;
        }

        public async Task<List<ProductionLine>> GetProductionLinesFilterLine(string LineFilter, DateTime StartTime, DateTime EndTime)
        {
            var listReturn = new List<ProductionLine>();
            string SPName = "[ProcessHistory].[GetPQCMesDataReadtime]";
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(SPName, conn))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Line", LineFilter);
                command.Parameters.AddWithValue("@InspectStart", StartTime.ToString());
                command.Parameters.AddWithValue("@InspectEnd", EndTime.ToString());
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var productLine = new ProductionLine()
                        {
                            Line = (string)rdr["Line"],
                            Product = (string)rdr["Product"],
                            InspectStart = (DateTime)rdr["StartTime"],
                            InspectEnd = (DateTime)rdr["EndTime"],
                            Output = (decimal)rdr["OPQty"],
                            NotGood = (decimal)rdr["NGQty"],
                            Rework = (decimal)rdr["RWQty"]
                        };
                        listReturn.Add(productLine);
                    }
                }
            }

            return listReturn;
        }

        public async Task<List<ProductionRealtime>> GetProductionRealtimes(string LineFilter, DateTime StartTime, DateTime EndTime)
        {
            var listReturn = new List<ProductionRealtime>();
            string SPName = "[ProcessHistory].[GetPQCMesDataRealtime]";
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(SPName, conn))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Line", LineFilter);
                command.Parameters.AddWithValue("@InspectStart", StartTime.ToString());
                command.Parameters.AddWithValue("@InspectEnd", EndTime.ToString());
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var realtime = new ProductionRealtime()
                        {
                            Line = (string)rdr["Line"],
                            Product = (string)rdr["Product"],
                            Date = (DateTime)rdr["Date"],
                            Hour = (int)rdr["Hour"],
                            PassedQty = (decimal)rdr["Passed"],
                            NotPassedQty = (decimal)rdr["NotPassed"],
                        };
                        listReturn.Add(realtime);
                    }
                }
            }

            return listReturn;
        }
    }
}
