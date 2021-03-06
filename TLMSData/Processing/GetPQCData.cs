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
                            Date = (DateTime)rdr["Date"],
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
                        productLine.DefectRate = (double)(productLine.NotGood / productLine.Actual) * 100;
                        productLine.ReworkRate = (double)(productLine.Rework / productLine.Actual) * 100;
                        productLine.OutputRate = (double)(productLine.Output / productLine.Actual) * 100;

                        var target = tLMSDataContext.DailyPerformanceGoals.Where(d => d.Model == productLine.Product)
                                                                 .Where(d => d.StartDate < DateTime.Now)
                                                                 .Where(d => d.EndDate == null || d.EndDate > DateTime.Now)
                                                                 .OrderByDescending(o => o.StartDate)
                                                                .FirstOrDefault();
                        if (target is null)
                        {
                            productLine.Status = ProductionStatus.NotSetTarget;
                            productLine.OPTarget = (target is null) ? 0 : target.OutputTarget;
                            productLine.NGTarget = (target is null) ? 0 : target.NotGoodTarget;
                            productLine.RWTarget = (target is null) ? 0 : target.ReworkTarget;
                            productLine.OpenQty = (target is null) ? 0 : (productLine.OPTarget - productLine.Actual);
                        }
                        else
                        {
                            productLine.OPTarget = target.OutputTarget;
                            productLine.NGTarget = target.NotGoodTarget;
                            productLine.RWTarget = target.ReworkTarget;
                            productLine.OpenQty = productLine.OPTarget - productLine.Actual;
                            var defectTargetRate = (double)(productLine.NGTarget / (productLine.OPTarget + productLine.RWTarget + productLine.NGTarget)) * 100;
                            var reworkTargetRate = (double)(productLine.RWTarget / (productLine.OPTarget + productLine.RWTarget + productLine.NGTarget)) * 100;
                            if (productLine.DefectRate >= defectTargetRate)
                            {
                                productLine.Status = ProductionStatus.HighDefect;
                            }
                            else if (productLine.ReworkRate >= reworkTargetRate)
                            {
                                productLine.Status = ProductionStatus.HighRework;
                            }
                            else
                            {
                                productLine.Status = ProductionStatus.GoodPerformance;
                            }
                        }

                        listReturn.Add(productLine);
                    }
                }
            }

            ProductionPerformance performance = new ProductionPerformance();
            performance.Throughput = (int)listReturn.Sum(d => d.Output);
            performance.Yield = listReturn.Sum(d => d.Actual) > 0
                                ? (double)Math.Round(listReturn.Sum(d => d.Output) / listReturn.Sum(d => d.Actual), 2) * 100
                                : 0;

            return new ProductionInformation()
            {
                productionLines = listReturn,
                performance = performance,
            };
        }

        public async Task<ProductionInformation> GetProductionInformationbyLine(string line, DateTime startTime, DateTime endTime)
        {
            var listReturn = new List<ProductionLine>();
            string SPName = "[ProcessHistory].[GetPQCMesDataFilterLine]";
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(SPName, conn))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Line", line);
                command.Parameters.AddWithValue("@InspectStart", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@InspectEnd", endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        var productLine = new ProductionLine()
                        {
                            Date = (DateTime)rdr["Date"],
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
                        productLine.DefectRate = (double)(productLine.NotGood / productLine.Actual) * 100;
                        productLine.ReworkRate = (double)(productLine.Rework / productLine.Actual) * 100;
                        productLine.OutputRate = (double)(productLine.Output / productLine.Actual) * 100;

                        var target = tLMSDataContext.DailyPerformanceGoals.Where(d => d.Model == productLine.Product)
                                                                 .Where(d => d.StartDate < DateTime.Now)
                                                                 .Where(d => d.EndDate == null || d.EndDate > DateTime.Now)
                                                                 .OrderByDescending(o => o.StartDate)
                                                                .FirstOrDefault();
                        if (target is null)
                        {
                            productLine.Status = ProductionStatus.NotSetTarget;
                            productLine.OPTarget = (target is null) ? 0 : target.OutputTarget;
                            productLine.NGTarget = (target is null) ? 0 : target.NotGoodTarget;
                            productLine.RWTarget = (target is null) ? 0 : target.ReworkTarget;
                            productLine.OpenQty = (target is null) ? 0 : (productLine.OPTarget - productLine.Actual);
                        }
                        else
                        {
                            productLine.OPTarget = target.OutputTarget;
                            productLine.NGTarget = target.NotGoodTarget;
                            productLine.RWTarget = target.ReworkTarget;
                            productLine.OpenQty = productLine.OPTarget - productLine.Actual;
                            var defectTargetRate = (double)(productLine.NGTarget / (productLine.OPTarget + productLine.RWTarget + productLine.NGTarget)) * 100;
                            var reworkTargetRate = (double)(productLine.RWTarget / (productLine.OPTarget + productLine.RWTarget + productLine.NGTarget)) * 100;
                            if (productLine.DefectRate >= defectTargetRate)
                            {
                                productLine.Status = ProductionStatus.HighDefect;
                            }
                            else if (productLine.ReworkRate >= reworkTargetRate)
                            {
                                productLine.Status = ProductionStatus.HighRework;
                            }
                            else if (productLine.Output >= productLine.OPTarget)
                            {
                                productLine.Status = ProductionStatus.Completed;
                            }
                            else
                            {
                                productLine.Status = ProductionStatus.GoodPerformance;
                            }
                        }

                        listReturn.Add(productLine);
                    }
                }
            }

            ProductionPerformance performance = new ProductionPerformance();
            performance.Throughput = (int)listReturn.Sum(d => d.Output);
            performance.Yield = listReturn.Sum(d => d.Actual) > 0
                                ? (double)Math.Round(listReturn.Sum(d => d.Output) / listReturn.Sum(d => d.Actual), 2) * 100
                                : 0;

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
                command.Parameters.AddWithValue("@InspectStart", StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@InspectEnd", EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
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
                command.Parameters.AddWithValue("@InspectStart", StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@InspectEnd", EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
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
                command.Parameters.AddWithValue("@InspectStart", StartTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@InspectEnd", EndTime.ToString("yyyy-MM-dd HH:mm:ss"));
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
                            Output = (decimal)rdr["OPQty"],
                            NotGood = (decimal)rdr["NGQty"],
                            Rework = (decimal)rdr["RWQty"],
                            PassedQty = (decimal)rdr["Passed"],
                            NotPassedQty = (decimal)rdr["NotPassed"],
                        };

                        realtime.performance.Throughput = (int)realtime.PassedQty;
                        realtime.performance.Yield = (realtime.PassedQty + realtime.NotPassedQty) > 0
                            ? Math.Round((double)realtime.PassedQty / ((double)(realtime.PassedQty + realtime.NotPassedQty)) * 100, 2)
                            : 0;

                        listReturn.Add(realtime);
                    }
                }
            }

            return listReturn;
        }
    }
}
