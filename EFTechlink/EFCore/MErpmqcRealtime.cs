using System;
using System.Collections.Generic;

#nullable disable

namespace EFTechlink.EFCore
{
    public partial class MErpmqcRealtime
    {
        public string Serno { get; set; }
        public string Lot { get; set; }
        public string Model { get; set; }
        public string Site { get; set; }
        public string Factory { get; set; }
        public string Line { get; set; }
        public string Process { get; set; }
        public string Item { get; set; }
        public DateTime Inspectdate { get; set; }
        public TimeSpan Inspecttime { get; set; }
        public string Data { get; set; }
        public string Judge { get; set; }
        public string Status { get; set; }
        public string Remark { get; set; }
    }
}
