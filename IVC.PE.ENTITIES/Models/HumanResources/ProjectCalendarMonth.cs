using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class ProjectCalendarMonth
    {
        public Guid Id { get; set; }

        public Guid ProjectCalendarId { get; set; }

        public ProjectCalendar ProjectCalendar { get; set; }

        [Required]
        public int Month { get; set; }

        public string YearMonthNumber { get; set; }

        public string Description { get; set; }

        [Required]
        public int ProcessType { get; set; }

        [Required]
        public DateTime MonthStart { get; set; }

        [Required]
        public DateTime MonthEnd { get; set; }

        public bool IsClosed { get; set; }
    }
}
