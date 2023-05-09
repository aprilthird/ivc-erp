using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class SewerGroupSchedule
    {
        public Guid Id { get; set; }

        public Guid ProjectCalendarWeekId { get; set; }
        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }

        public Guid WorkFrontHeadId { get; set; }
        public WorkFrontHead WorkFrontHead { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public bool isIssued { get; set; }
    }
}
