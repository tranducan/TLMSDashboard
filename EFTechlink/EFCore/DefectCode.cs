using System;
using System.Collections.Generic;

#nullable disable

namespace EFTechlink.EFCore
{
    public partial class DefectCode
    {
        public long DefectCodeId { get; set; }
        public string Site { get; set; }
        public string Line { get; set; }
        public string Process { get; set; }
        public string Code { get; set; }
        public string MesdefectCode { get; set; }
        public string Description { get; set; }
    }
}
