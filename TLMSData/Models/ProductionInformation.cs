using System;
using System.Collections.Generic;
using System.Text;

namespace TLMSData.Models
{
    public class ProductionInformation
    {
        public ProductionInformation()
        {
            productionLines = new List<ProductionLine>();
        }

        public List<ProductionLine> productionLines { get; set; }

        public ProductionPerformance performance { get; set; } 
    }
}
