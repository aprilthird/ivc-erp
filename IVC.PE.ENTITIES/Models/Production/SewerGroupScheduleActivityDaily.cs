using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class SewerGroupScheduleActivityDaily
    {
        public Guid Id { get; set; }

        public Guid SewerGroupScheduleActivityId { get; set; }
        public SewerGroupScheduleActivity SewerGroupScheduleActivity { get; set; }

        public DateTime ReportDate { get; set; }

        public double FootageDaily { get; set; }
    }
}
