using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Logistics.ViewModels.MeasurementUnitViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.ViewModels.SewerGroupScheduleViewModels
{
    public class SewerGroupScheduleActivityViewModel
    {
        public Guid? Id { get; set; }

        public Guid SewerGroupScheduleId { get; set; }
        public SewerGroupScheduleViewModel SewerGroupSchedule { get; set; }

        public Guid SewerManifoldId { get; set; }
        public SewerManifoldViewModel SewerManifold { get; set; }

        public Guid ProjectFormulaActivityId { get; set; }
        public ProjectFormulaActivityViewModel ProjectFormulaActivity { get; set; }
    }

    public class SewerGroupScheduleActivityFullViewModel
    {
        public Guid? Id { get; set; }

        public Guid SewerGroupScheduleId { get; set; }
        public SewerGroupScheduleViewModel SewerGroupSchedule { get; set; }

        [Display(Name = "Tramo", Prompt = "Tramo")]
        public Guid SewerManifoldId { get; set; }
        public SewerManifoldViewModel SewerManifold { get; set; }

        [Display(Name = "Actividad", Prompt = "Actividad")]
        public Guid ProjectFormulaActivityId { get; set; }
        public ProjectFormulaActivityViewModel ProjectFormulaActivity { get; set; }

        [Display(Name = "Metrado (Lunes)", Prompt = "Metrado (Lunes)")]
        public double? FootageMonday { get; set; } = 0;

        [Display(Name = "Metrado (Martes)", Prompt = "Metrado (Martes)")]
        public double? FootageTuesday { get; set; } = 0;

        [Display(Name = "Metrado (Miércoles)", Prompt = "Metrado (Miércoles)")]
        public double? FootageWednesday { get; set; } = 0;

        [Display(Name = "Metrado (Jueves)", Prompt = "Metrado (Jueves)")]
        public double? FootageThrusday { get; set; } = 0;

        [Display(Name = "Metrado (Viernes)", Prompt = "Metrado (Viernes)")]
        public double? FootageFriday { get; set; } = 0;

        [Display(Name = "Metrado (Sábado)", Prompt = "Metrado (Sábado)")]
        public double? FootageSaturday { get; set; } = 0;

        public double FootageTotal => (FootageMonday ?? 0 ) + (FootageTuesday ?? 0 )+ (FootageWednesday ?? 0) + (FootageThrusday ?? 0) + (FootageFriday ?? 0) + (FootageSaturday ?? 0);
    }

    public class SewerGroupScheduleExcelViewModel
    {
        public Guid Id { get; set; }
        public string FormulaName { get; set; }
        public string SewerManifoldAddress { get; set; }
        public string SewerManifoldName { get; set; }
        public string ActivityDescription { get; set; }
        public string WorkFrontHeadName { get; set; }
        public string WorkFrontHeadContactNumber { get; set; }
        public string SewerGroupCode { get; set; }
        public Guid? SewerGroupForemanId { get; set; }
        public string SewerGroupForemanName { get; set; }
        public string SewerGroupForemanContactNumber { get; set; }
        public string MeasurementUnitAbbrevitation { get; set; }
        public double? FootageMonday { get; set; } = 0;
        public double? FootageTuesday { get; set; } = 0;
        public double? FootageWednesday { get; set; } = 0;
        public double? FootageThrusday { get; set; } = 0;
        public double? FootageFriday { get; set; } = 0;
        public double? FootageSaturday { get; set; } = 0;
        public double FootageTotal => (FootageMonday ?? 0) + (FootageTuesday ?? 0) + (FootageWednesday ?? 0) + (FootageThrusday ?? 0) + (FootageFriday ?? 0) + (FootageSaturday ?? 0);

    }
}
