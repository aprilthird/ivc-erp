using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class ProjectCalendarWeekFile
    {

        public Guid Id { get; set; }

        public Guid ProjectCalendarWeekId { get; set; }

        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }

        public Uri FileUrl { get; set; }
    }
}
