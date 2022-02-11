using EFTechlink.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TLMSData.Interfacing;
using EFTechlink.EFCore;
using EFTechlink.Model;
using System.Linq;
namespace TLMSData.Processing
{
    public class MqcDataInteracting : IMqcDataInteracting
    {
        private readonly TLMSDataContext dataContext;

        public MqcDataInteracting(
            TLMSDataContext context)
        {
            this.dataContext = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<MqcDataSummary> GetMqcDataSummary(DateTime dateTimeStart, DateTime dateTimeEnd)
        {
            var mqcDataTemp = this.dataContext.MErpmqcRealtimes
                                        .Take(100)
                                        //.Where(d => (d.Inspectdate + d.Inspecttime) >= dateTimeStart
                                        //          && (d.Inspectdate + d.Inspecttime) <= dateTimeEnd)
                                        .ToList();
            if ((mqcDataTemp is null) || mqcDataTemp.Count == 0)
            {
                return null;
            }

            return new MqcDataSummary()
            {
                Department = "MQC",
                Factory = mqcDataTemp[0].Factory,
                Site = mqcDataTemp[0].Site,
                Line = mqcDataTemp[0].Line,
                Model = mqcDataTemp[0].Model,
                Lot = mqcDataTemp[0].Lot,
                DatetimeStart = dateTimeStart,
                DateTimeEnd = dateTimeEnd,
                TotalGoodQty = mqcDataTemp
                                .Where(d => d.Remark == "OP")
                                .Sum(d => (long.Parse(d.Data))),
                TotalNotGoodQty = mqcDataTemp
                                .Where(d => d.Remark == "NG")
                                .Sum(d => (long.Parse(d.Data))),
                TotalReworkQty = mqcDataTemp
                                .Where(d => d.Remark == "RW")
                                .Sum(d => (long.Parse(d.Data))),
            };
        }
    }
}
