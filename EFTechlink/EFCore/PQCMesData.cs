using System;
using System.Collections.Generic;

#nullable disable

namespace EFTechlink.EFCore
{
    public partial class PQCMesData
    {
        public long PqcmesDataId { get; set; }
        public string Pocode { get; set; }
        public string LotNumber { get; set; }
        public string Model { get; set; }
        public string Site { get; set; }
        public string Line { get; set; }
        public string Process { get; set; }
        public string Attribute { get; set; }
        public string AttributeType { get; set; }
        public decimal Quantity { get; set; }
        public string Flag { get; set; }
        public string Inspector { get; set; }
        public DateTime InspectDateTime { get; set; }
        public DateTime LastTimeModified { get; set; }
        public string LastModifiedUser { get; set; }
        public byte[] VersionNumber { get; set; }
    }
}
