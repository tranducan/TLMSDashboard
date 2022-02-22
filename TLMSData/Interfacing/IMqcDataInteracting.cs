using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EFTechlink.EFCore;
using EFTechlink.Model;

namespace TLMSData.Interfacing
{
    public interface IMqcDataInteracting
    {
        /// <summary>
        /// Get MQC data summary from MQCReadtime table and filter to Datetime Start
        /// Datetime End
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        Task<MqcDataSummary> GetMqcDataSummary(DateTime dateTimeStart, DateTime dateTimeEnd);

        /// <summary>
        /// Get MQC data summary from MQCReadtime table and filter to Datetime Start
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        Task<PqcDataSummary> GetPqcDataSummary(DateTime dateTimeStart, DateTime dateTimeEnd);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTimeStart"></param>
        /// <param name="dateTimeEnd"></param>
        /// <returns></returns>
        Task<List<PQCMesData>> GetPqcData(DateTime dateTimeStart, DateTime dateTimeEnd);

    }
}
