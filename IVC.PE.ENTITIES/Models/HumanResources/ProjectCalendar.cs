using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.HumanResources
{
    public class ProjectCalendar
    {
        public Guid Id { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public bool IsWeekly { get; set; } //True = Semanal, False = Mensual

        public DateTime? FirstDayOfThCalendar { get; set; } //0:Dom, 1:Lun, ...., 6:Sab

        [Required]
        public Guid ProjectId { get; set; }

        public Project Project { get; set; }
    }
}
