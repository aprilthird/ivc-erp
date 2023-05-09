using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.PayrollViewModels
{
    public class PayrollCalendarViewModel
    {
        public Guid? Id { get; set; }

        [Required(ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Range(0, 9999, ErrorMessage = "Ingrese un año válido")]
        [Display(Name = "Año", Prompt = "Año")]
        public int Year { get; set; }

        [Required]
        [Display(Name = "¿Es Semanal?", Prompt = "¿Es Semanal?")]
        public bool IsWeekly { get; set; } //True = Semanal, False = Mensual

        [Required(AllowEmptyStrings = true)]
        [Display(Name = "Inicia el", Prompt = "Inicia el")]
        public string FirstDayOfThCalendar { get; set; }

        [Required]
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }

        public PayrollCalendarWeekViewModel ProjectCalendarWeek { get; set; }

        public IEnumerable<PayrollCalendarWeekViewModel> ProjectCalendarWeeks { get; set; }

        public PayrollCalendarMonthViewModel ProjectCalendarMonth { get; set; }

        public IEnumerable<PayrollCalendarMonthViewModel> ProjectCalendarMonths { get; set; }
    }

    public class PayrollCalendarWeekViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectCalendarId { get; set; }

        public PayrollCalendarViewModel ProjectCalendar { get; set; }

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

    public class PayrollCalendarMonthViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        public Guid ProjectCalendarId { get; set; }

        public PayrollCalendarViewModel ProjectCalendar { get; set; }

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
