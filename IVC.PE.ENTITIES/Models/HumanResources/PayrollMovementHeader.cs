using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class PayrollMovementHeader
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public Guid? ProjectCalendarWeekId { get; set; }
        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }

        public int ProcessStatus { get; set; }

        public IEnumerable<PayrollMovementDetail> PayrollMovementDetails { get; set; }
    }
}
