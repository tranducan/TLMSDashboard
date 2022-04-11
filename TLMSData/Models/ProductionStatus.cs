using System;
using System.Collections.Generic;
using System.Text;

namespace TLMSData.Models
{
    public enum ProductionStatus
    {
        NotStarted,

        NotSetTarget,//blink

        HighDefect,//blink

        HighRework, //blink

        GoodPerformance,

        Completed
    }
}
