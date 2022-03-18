using System;
using System.Collections.Generic;
using System.Text;

namespace TLMSData.Models
{
    public class ProductTarget
    {
        public string ProductName { get; set; }

        public string Process { get; set; }

        public string Site { get; set; }

        public decimal OutputTarget { get; set; }

        public decimal ReworkTarget { get; set; }

        public decimal NotGoodTarget { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

    }
}
