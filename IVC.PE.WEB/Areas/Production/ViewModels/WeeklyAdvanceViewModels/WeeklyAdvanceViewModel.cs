using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.ProjectCalendarWeekViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Production.ViewModels.WeeklyAdvanceViewModels
{
    public class WeeklyAdvanceViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Semana")]
        public Guid ProjectCalendarWeekId { get; set; }
        public ProjectCalendarWeekViewModel ProjectCalendarWeek { get; set; }

        [Display(Name = "Fórmula")]
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormulaViewModel ProjectFormula { get; set; }

        [Display(Name = "Jefe de Frente")]
        public Guid WorkFrontHeadId { get; set; }
        public WorkFrontHeadViewModel WorkFrontHead { get; set; }

        [Display(Name = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        public string TotalNetBudget { get; set; }

        public string AccumulatedBudget { get; set; }

        public string PercentageAdvance { get; set; }

        public int WorkersNumberOP { get; set; }

        public int WorkersNumberOF { get; set; }

        public int WorkersNumberPE { get; set; }

        public int WorkerNumberTotal { get; set; }

        public string SaleMO { get; set; }

        public string SaleEQ { get; set; }

        public string SaleSubcontract { get; set; }

        public string SaleMaterials { get; set; }

        public string SaleTotal { get; set; }

        public string GoalMO { get; set; }

        public string GoalEQ { get; set; }

        public string GoalSubcontract { get; set; }

        public string GoalMaterials { get; set; }

        public string GoalTotal { get; set; }

        public string CostMO { get; set; }

        public string CostEQ { get; set; }

        public string CostSubcontract { get; set; }

        public string CostMaterials { get; set; }

        public string CostTotal { get; set; }

        public string MarginActual { get; set; }

        public string MarginAccumulated { get; set; }
    }
}
