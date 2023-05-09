using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.MeteredsRestatedByStreetchViewModels
{
    public class MeteredsRestatedByStreetchViewModel
    {
    public Guid? Id { get; set; }

    [Display(Name="Títulos de Presupuestos")]
    public Guid BudgetTitleId { get; set; }
    public BudgetTitleViewModel BudgetTitle { get; set; }

    [Display(Name ="Fórmula")]
    public Guid ProjectFormulaId { get; set; }
    public ProjectFormulaViewModel ProjectFormula { get; set; }

    [Display(Name ="Item")]
    public string ItemNumber { get; set; }

    [Display(Name="Descripción")]
    public string Description { get; set; }

    [Display(Name ="Unidad")]
    public string Unit { get; set; }

    [Display(Name="Metrado")]
    public string Metered { get; set; }

    [Display(Name ="Frente de trabajo")]
    public Guid? WorkFrontId { get; set; }
    public WorkFrontViewModel WorkFront { get; set; }

    [Display(Name ="Cuadrilla")]
    public Guid? SewerGroupId { get; set; }
    public SewerGroupViewModel SewerGroup { get; set; }
}
}
