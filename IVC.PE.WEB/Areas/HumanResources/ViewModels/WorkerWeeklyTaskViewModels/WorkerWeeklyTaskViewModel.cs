using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerDailyTaskViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerWeeklyTaskViewModels
{
    public class WorkerWeeklyTaskViewModel
    {
        public Guid WorkerId { get; set; }

        public string Document { get; set; }

        public string WorkerFullName { get; set; }

        public string WorkPosition { get; set; }

        public string Category { get; set; }

        public string Origin { get; set; }

        public string EntryDate { get; set; }
        public DateTime EntryDateDt { get; set; }

        public DateTime? CeaseDateDt { get; set; }

        public string WorkerTypeDocNumber { get; set; }

        public string WorkFrontHeadName { get; set; }

        public string SewerGroupCode { get; set; }

        public WorkerWeeklyDayTaskViewModel[] WorkerDailyTasks { get; set; }

        public decimal HoursVal { get; set; }
        public decimal Hours60Val { get; set; }
        public decimal Hours100Val { get; set; }
    }

    public class WorkerWeeklyDayTaskViewModel
    {
        public string Hours { get; set; }
        public string Hours60 { get; set; }
        public string Hours100 { get; set; }
        public string Phase { get; set; }
    }
}
