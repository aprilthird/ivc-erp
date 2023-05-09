using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarMonthViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarViewModels
{
    public class ProjectCalendarViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Range(0, 9999, ErrorMessage = "Ingrese un año válido")]
        [Display(Name = "Año", Prompt = "Año")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "¿Es Semanal?", Prompt = "¿Es Semanal?")]
        public bool IsWeekly { get; set; } //True = Semanal, False = Mensual

        [Required(AllowEmptyStrings =true)]
        [Display(Name = "Inicia el", Prompt = "Inicia el")]
        public string FirstDayOfThCalendar { get; set; }

        [Required]
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }

        public ProjectCalendarWeekViewModel ProjectCalendarWeek { get; set; }

        public IEnumerable<ProjectCalendarWeekViewModel> ProjectCalendarWeeks { get; set; }

        public ProjectCalendarMonthViewModel ProjectCalendarMonth { get; set; }

        public IEnumerable<ProjectCalendarMonthViewModel> ProjectCalendarMonths { get; set; }
    }
}
