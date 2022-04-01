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

        public List<DefectItems> Notgood5Items { get; set; }

        public List<DefectItems> NotgoodItems { get; set; }

        public List<DefectItems> Rework5Items { get; set; }

        public List<DefectItems> ReworkItems { get; set; }

        public decimal NGOtherQuantity { get; set; }

        public decimal RWOtherQuantity { get; set; }
    }
}
