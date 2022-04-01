using System;
using System.Collections.Generic;
using System.Text;

namespace TLMSData.Models
{
    public class ProductionSummary
    {
        public ProductionSummary()
        {
            ProductionLine = new ProductionLine();
            NotgoodItems = new List<DefectItems>();
            ReworkItems = new List<DefectItems>();
        }

        public ProductionLine ProductionLine { get; set; }

        public List<DefectItems> NotgoodItems { get; set; }

        public List<DefectItems> ReworkItems { get; set; }
    }
}
