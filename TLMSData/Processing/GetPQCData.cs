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
    public class GetPQCData : IGetPQCData
    {

        private readonly string ConnectionString;
        private readonly TLMSDataContext tLMSDataContext;

        public GetPQCData(
        TLMSDataContext dataContext)
        {
            tLMSDataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            ConnectionString = tLMSDataContext.Database.GetDbConnection().ConnectionString;
        }

        public async Task<ProductionInformation> GetProductionInformation(DateTime startTime, DateTime endTime)
        {
            var listReturn = new List<ProductionLine>();
            string SPName = "[ProcessHistory].[GetPQCMesData]";
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(SPName, conn))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectStart", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@InspectEnd", endTime.ToString("yyyy-MM-dd HH:mm:ss"));
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
                            Rework = (decimal)rdr["RWQty"],

                        };

                        productLine.Actual = productLine.Output + productLine.NotGood + productLine.Rework;
                        productLine.ProductionRunning = productLine.InspectEnd - productLine.InspectStart;
                        productLine.OpenQty = productLine.OPTarget - productLine.Actual;

                        var target = tLMSDataContext.DailyPerformanceGoals.Where(d => d.Model == productLine.Product)
                                                                 .Where(d => d.StartDate < DateTime.Now)
                                                                 .Where(d => d.EndDate == null || d.EndDate > DateTime.Now)
                                                                .FirstOrDefault();
                        productLine.OPTarget = target.OutputTarget;
                        productLine.NGTarget = target.NotGoodTarget;
                        productLine.RWTarget = target.ReworkTarget;
                        productLine.OpenQty = productLine.OPTarget - productLine.Actual;

                        listReturn.Add(productLine);
                    }
                }
            }

            ProductionPerformance performance = new ProductionPerformance();
            performance.Throughput = (int)listReturn.Sum(d => d.Output);
            performance.Yield = (double)Math.Round(listReturn.Sum(d => d.Output) / listReturn.Sum(d => d.Actual), 2) * 100;

            return new ProductionInformation()
            {
                productionLines = listReturn,
                performance = performance,
            };
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
            string SPName = "[ProcessHistory].[GetPQCMesDataFilterLine]";
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
