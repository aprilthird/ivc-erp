using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SystemPhaseViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerDailyTaskViewModels
{
    public class WorkerDailyTaskViewModel
    {
        public Guid? Id { get; set; }

        [Required]
        [Display(Name = "Obrero", Prompt = "Obrero")]
        public Guid WorkerId { get; set; }

        public WorkerViewModel Worker { get; set; }

        [Display(Name = "Fase", Prompt = "Fase")]
        public Guid? ProjectPhaseId { get; set; }

        public ProjectPhaseViewModel ProjectPhase { get; set; }

        [Required]
        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }

        public SewerGroupViewModel SewerGroup { get; set; }

        [Required]
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string Date { get; set; }

        [Display(Name = "H. Normales", Prompt = "H. Normales")]
        public decimal HoursNormal { get; set; }

        [Display(Name = "H. Extras 60%", Prompt = "H. Extras 60%")]
        public decimal Hours60Percent { get; set; }

        [Display(Name = "H. Extras 100%", Prompt = "H. Extras 100%")]
        public decimal Hours100Percent { get; set; }

        [Display(Name = "H. Desc. Med.", Prompt = "H. Desc. Med.")]
        public decimal HoursMedicalRest { get; set; }

        [Display(Name = "Subs. Med.", Prompt = "Subs. Med.")]
        public bool MedicalLeave { get; set; }

        [Display(Name = "H. Lic. Paternidad", Prompt = "H. Lic. Paternidad")]
        public decimal HoursPaternityLeave { get; set; }

        [Display(Name = "H. Feriado", Prompt = "H. Feriado")]
        public decimal HoursHoliday { get; set; }

        [Display(Name = "H. Lic. Con Goce", Prompt = "H. Lic. Con Goce")]
        public decimal HoursPaidLeave { get; set; }

        [Display(Name = "Lic. Sin Goce", Prompt = "Lic. Sin Goce")]
        public bool UnPaidLeave { get; set; }

        [Display(Name = "Suspensión Lab.", Prompt = "Suspensión Lab.")]
        public bool LaborSuspension { get; set; }

        [Display(Name = "Inasistencia", Prompt = "Inasistencia")]
        public bool NonAttendance { get; set; }

        [Display(Name = "Cesar", Prompt = "Cesar")]
        public bool IsCeased { get; set; }

        [Display(Name = "Fecha de Cese", Prompt = "Fecha de Cese")]
        public string CeasedDate { get; set; }
    }

    public class WorkerDailyTasksImportDayViewModel
    {
        public Guid ProjectId { get; set; }

        [Display(Name ="Jefe de Frente", Prompt = "Jefe de Frente")]
        public Guid WorkFrontHeadId { get; set; }

        [Display(Name = "Día", Prompt = "Día")]
        public string DateTask { get; set; }

        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }

    public class WorkerDailyTaskCopyDayViewModel
    {
        public Guid ProjectId { get; set; }

        [Display(Name = "Jefe de Frente", Prompt = "Jefe de Frente")]
        public Guid? WorkFrontHeadId { get; set; }

        [Display(Name = "Copiar de:", Prompt = "Copiar de:")]
        public string FromDay { get; set; }

        [Display(Name = "Copiar a:", Prompt = "Copiar a:")]
        public string ToDay { get; set; }
    }

    public class WorkerDailyTaskCreateViewModel
    {
        [Required]
        [Display(Name = "Obrero", Prompt = "Obrero")]
        public Guid WorkerId { get; set; }

        public WorkerViewModel Worker { get; set; }

        public ProjectPhaseViewModel ProjectPhase { get; set; }

        [Required]
        [Display(Name = "Proyecto", Prompt = "Proyecto")]
        public Guid ProjectId { get; set; }

        public ProjectViewModel Project { get; set; }

        [Required]
        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }

        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string Date { get; set; }
    }
}
