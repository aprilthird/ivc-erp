using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class ProjectCalendarWeek
    {
        public Guid Id { get; set; }

        public Guid ProjectCalendarId { get; set; }

        public ProjectCalendar ProjectCalendar { get; set; }
        
        public int WeekNumber { get; set; }

        public string YearWeekNumber { get; set; }

        [Required]
        public int Month { get; set; }

        [Required]
        public int Year { get; set; }

        public string Description { get; set; }

        [Required]
        public int ProcessType { get; set; } //0:Normal, 1:Gratificación, 2:Reintegro

        [Required]
        public DateTime WeekStart { get; set; }

        [Required]
        public DateTime WeekEnd { get; set; }

        public bool IsClosed { get; set; }
    }
}