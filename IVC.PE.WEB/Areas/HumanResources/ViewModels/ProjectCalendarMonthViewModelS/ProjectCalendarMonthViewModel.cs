using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarMonthViewModels
{
    public class ProjectCalendarMonthViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid ProjectCalendarId { get; set; }

        public ProjectCalendarViewModel ProjectCalendar { get; set; }

        [Required]
        public int Month { get; set; }

        public string YearMonthNumber { get; set; }

        public string Description { get; set; }

        [Required]
        public int ProcessType { get; set; }

        [Required]
        public string MonthStart { get; set; }

        [Required]
        public string MonthEnd { get; set; }

        public bool IsClosed { get; set; }
    }
}
