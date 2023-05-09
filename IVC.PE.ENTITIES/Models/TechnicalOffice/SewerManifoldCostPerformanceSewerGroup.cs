using IVC.PE.ENTITIES.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerManifoldCostPerformanceSewerGroup
    {
        public Guid Id { get; set; }

        public Guid SewerManifoldCostPerformanceId { get; set; }
        public SewerManifoldCostPerformance SewerManifoldCostPerformance { get; set; }

        public Guid ProjectCalendarWeekId { get; set; }
        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public double WorkforceEquipment { get; set; }
        public double WorkforceEquipmentService { get; set; }

        public double SecurityFactor { get; set; }
    }
}
