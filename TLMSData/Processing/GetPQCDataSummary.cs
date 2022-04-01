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
    public class GetPQCDataSummary : IGetPQCDataSummary
    {
        private readonly string ConnectionString;
        private readonly TLMSDataContext tLMSDataContext;

        public GetPQCDataSummary(
            TLMSDataContext dataContext)
        {
            tLMSDataContext = dataContext ?? throw new ArgumentNullException(nameof(dataContext));
            ConnectionString = tLMSDataContext.Database.GetDbConnection().ConnectionString;
        }

        public async Task<List<ProductionSummary>> GetProductionSummary(DateTime startTime, DateTime endTime)
        {
            var listReturn = new List<ProductionSummary>();
            string spName = "ProcessHistory.GetPQCMesDataSummary";
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(spName, conn))
            {
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InspectStart", startTime.ToString("yyyy-MM-dd HH:mm:ss"));
                command.Parameters.AddWithValue("@InspectEnd", endTime.ToString("yyyy-MM-dd HH:mm:ss"));
                conn.Open();
                using (SqlDataReader rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {                   
                        List<DefectItems> defectItems = new List<DefectItems>();
                        for (int i = 1; i <= 35; i++)
                        {
                            DefectItems item = new DefectItems();
                            item.DefectCode = "NG" + i.ToString();
                            item.Quantity = (decimal)rdr[item.DefectCode];
                            defectItems.Add(item);
                        }

                        List<DefectItems> reworkItems = new List<DefectItems>();
                        for (int i = 1; i <= 35; i++)
                        {
                            DefectItems item = new DefectItems();
                            item.DefectCode = "RW" + i.ToString();
                            item.Quantity = (decimal)rdr[item.DefectCode];
                            reworkItems.Add(item);
                        }

                        var productionLine = new ProductionLine()
                        {
                            InspectStart = (DateTime)rdr["StartTime"],
                            InspectEnd = (DateTime)rdr["EndTime"],
                            Line = rdr["Line"].ToString(),
                            Product = rdr["Product"].ToString(),
                            Lot = rdr["Lot"].ToString(),
                            Output = (decimal)rdr["Output"],
                            NotGood = defectItems.Sum(d => d.Quantity),
                            Rework = reworkItems.Sum(d => d.Quantity),
                            ProductionRunning = (DateTime)rdr["EndTime"] - (DateTime)rdr["StartTime"],
                        };

                        var productionSummary = new ProductionSummary()
                        {
                            ProductionLine = productionLine,
                            NotgoodItems = defectItems,
                            ReworkItems = reworkItems,
                            Notgood5Items = defectItems.OrderByDescending(d => d.Quantity).Take(5).ToList(),
                            Rework5Items = reworkItems.OrderByDescending(d => d.Quantity).Take(5).ToList(),
                        };

                        productionSummary.NGOtherQuantity = productionLine.NotGood - productionSummary.Notgood5Items.Sum(d => d.Quantity);
                        productionSummary.RWOtherQuantity = productionLine.Rework - productionSummary.Rework5Items.Sum(d => d.Quantity);

                        listReturn.Add(productionSummary);
                    }
                }
            }

            return listReturn;
        }

        public async Task<List<ProductionSummary>> GetProductionSummarybyLine(string line, DateTime startTime, DateTime endTime)
        {
            var listReturn = new List<ProductionSummary>();
            string spName = "ProcessHistory.GetPQCMesDataLineSummary";
            using (var conn = new SqlConnection(ConnectionString))
            using (var command = new SqlCommand(spName, conn))
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
                        List<DefectItems> defectItems = new List<DefectItems>();
                        for (int i = 1; i <= 35; i++)
                        {
                            DefectItems item = new DefectItems();
                            item.DefectCode = "NG" + i.ToString();
                            item.Quantity = (decimal)rdr[item.DefectCode];
                            defectItems.Add(item);
                        }

                        List<DefectItems> reworkItems = new List<DefectItems>();
                        for (int i = 1; i <= 35; i++)
                        {
                            DefectItems item = new DefectItems();
                            item.DefectCode = "RW" + i.ToString();
                            item.Quantity = (decimal)rdr[item.DefectCode];
                            reworkItems.Add(item);
                        }

                        var productionLine = new ProductionLine()
                        {
                            InspectStart = (DateTime)rdr["StartTime"],
                            InspectEnd = (DateTime)rdr["EndTime"],
                            Line = rdr["Line"].ToString(),
                            Product = rdr["Product"].ToString(),
                            Lot = rdr["Lot"].ToString(),
                            Output = (decimal)rdr["Output"],
                            NotGood = defectItems.Sum(d => d.Quantity),
                            Rework = reworkItems.Sum(d => d.Quantity),
                            ProductionRunning = (DateTime)rdr["EndTime"] - (DateTime)rdr["StartTime"],
                        };

                        var productionSummary = new ProductionSummary()
                        {
                            ProductionLine = productionLine,
                            NotgoodItems = defectItems,
                            ReworkItems = reworkItems,
                            Notgood5Items = defectItems.OrderByDescending(d => d.Quantity).Take(5).ToList(),
                            Rework5Items = reworkItems.OrderByDescending(d => d.Quantity).Take(5).ToList(),
                        };

                        productionSummary.NGOtherQuantity = productionLine.NotGood - productionSummary.Notgood5Items.Sum(d => d.Quantity);
                        productionSummary.RWOtherQuantity = productionLine.Rework - productionSummary.Rework5Items.Sum(d => d.Quantity);

                        listReturn.Add(productionSummary);
                    }
                }
            }

            return listReturn;
        }
    }
}
