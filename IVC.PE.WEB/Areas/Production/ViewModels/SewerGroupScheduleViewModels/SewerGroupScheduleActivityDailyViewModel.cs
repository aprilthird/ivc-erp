using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.ViewModels.SewerGroupScheduleViewModels
{
    public class SewerGroupScheduleActivityDailyViewModel
    {
        public Guid? Id { get; set; }

        public Guid SewerGroupScheduleActivityId { get; set; }
        public SewerGroupScheduleActivityViewModel SewerGroupScheduleActivity { get; set; }

        public string ReportDate { get; set; }

        public double FootageDaily { get; set; }
    }
}
