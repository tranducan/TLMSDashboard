using System;
using System.Collections.Generic;
using System.Text;

namespace EFTechlink.Model
{
    public class PqcDataSummary
    {
        public string Department { get; set; }

        public string Factory { get; set; }

        public string Site { get; set; }

        public string Line { get; set; }

        public string Model { get; set; }

        public string Lot { get; set; }

        public string QRLotNo { get; set; }

        public DateTime DatetimeStart { get; set; }

        public DateTime DateTimeEnd { get; set; }

        public long TotalGoodQty { get; set; }

        public long TotalNotGoodQty { get; set; }

        public long TotalReworkQty { get; set; }
    }
}
