using System;
using System.Collections.Generic;
using System.Text;

namespace TLMSData.Models
{
    public class ProductionAnalysis
    {
        public string Line { get; set; }

        public string Lot { get; set; }

        public string Product { get; set; }

        public decimal OPTarget { get; set; }

        public decimal NGTarget { get; set; }

        public decimal RWTarget { get; set; }

        public decimal Actual { get; set; }

        public decimal OpenQty { get; set; }

        public decimal Output { get; set; }

        public decimal NotGood { get; set; }

        public decimal Rework { get; set; }

        public double DefectRate { get; set; }

        public double ReworkRate { get; set; }

        public double OutputRate { get; set; }

        public DateTime Date { get; set; }

        public string Shift { get; set; }

        public DateTime InspectStart { get; set; }

        public DateTime InspectEnd { get; set; }

        public TimeSpan ProductionRunning { get; set; }

        public ProductionEvaluation evaluation { get; set; }
    }

}
