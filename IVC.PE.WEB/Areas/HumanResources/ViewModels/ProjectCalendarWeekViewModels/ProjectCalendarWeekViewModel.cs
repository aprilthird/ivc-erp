using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels
{
    public class ProjectCalendarWeekViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectCalendarId { get; set; }

        public ProjectCalendarViewModel ProjectCalendar { get; set; }

        [Display(Name = "Nº de Semana", Prompt = "Nº de Semana")]
        public int WeekNumber { get; set; }

        public string YearWeekNumber { get; set; }

        [Required]
        [Display(Name = "Se declara en", Prompt = "Se declara en")]
        public int Month { get; set; }

        [Display(Name = "Descripción", Prompt = "Descripción")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Tipo de Proceso", Prompt = "Tipo de Proceso")]
        public int ProcessType { get; set; } //1: Normal, 2:Gratificación, 3:Reintegros

        [Required]
        [Display(Name = "Inicia el", Prompt = "Inicia el")]
        public string WeekStart { get; set; }

        [Required]
        [Display(Name = "Termina el", Prompt = "Termina el")]
        public string WeekEnd { get; set; }

        [Display(Name = "Estado", Prompt = "Estado")]
        public bool IsClosed { get; set; }
    }
}
