using System;
using System.Collections.Generic;

#nullable disable

namespace EFTechlink.EFCore
{
    public partial class DailyPerformanceGoal
    {
        public long DailyPerformanceGoaId { get; set; }
        public string Model { get; set; }
        public string Site { get; set; }
        public string Line { get; set; }
        public string Process { get; set; }
        public decimal OutputTarget { get; set; }
        public decimal NotGoodTarget { get; set; }
        public decimal ReworkTarget { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
}
