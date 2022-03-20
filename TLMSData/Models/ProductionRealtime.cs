using System;
using System.Collections.Generic;
using System.Text;

namespace TLMSData.Models
{
    public class ProductionRealtime
    {
        public ProductionRealtime()
        {
            performance = new ProductionPerformance();
        }

        public DateTime Date { get; set; }

        public int Hour { get; set; }

        public string Line { get; set; }

        public string Product { get; set; }

        public decimal PassedQty { get; set; }

        public decimal NotPassedQty { get; set; }

        public decimal Output { get; set; }

        public decimal NotGood { get; set; }

        public decimal Rework { get; set; }

        public ProductionPerformance performance { get; set; }

    }
}
