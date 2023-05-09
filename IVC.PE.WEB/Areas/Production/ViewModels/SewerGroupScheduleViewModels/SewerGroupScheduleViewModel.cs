using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.ViewModels.SewerGroupScheduleViewModels
{
    public class SewerGroupScheduleViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Semana", Prompt = "Semana")]
        public Guid ProjectCalendarWeekId { get; set; }
        public ProjectCalendarWeekViewModel ProjectCalendarWeek { get; set; }

        [Display(Name = "Jefe de Frente", Prompt = "Jefe de Frente")]
        public Guid WorkFrontHeadId { get; set; }
        public WorkFrontHeadViewModel WorkFrontHead { get; set; }

        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public string Description { get; set; }
        public bool IsIssued { get; set; }
    }
}
