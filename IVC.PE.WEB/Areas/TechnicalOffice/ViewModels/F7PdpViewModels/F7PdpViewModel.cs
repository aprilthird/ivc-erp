using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontHeadViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.FoldingF7ViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SewerManifoldViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.F7PdpViewModels
{
    public class F7PdpViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Formula", Prompt = "Formula")]
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormulaViewModel ProjectFormula { get; set; }

        [Display(Name = "Fecha de Reporte", Prompt = "Fecha de Reporte")]
        public string ReportDate { get; set; }

        [Display(Name = "Jefe de Frente", Prompt = "Jefe de Frente")]
        public Guid WorkFrontHeadId { get; set; }
        public WorkFrontHeadViewModel WorkFrontHead { get; set; }

        [Display(Name = "Frente", Prompt = "Frente")]
        public Guid? WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }

        [Display(Name = "Cuadrilla", Prompt = "Cuadrilla")]
        public Guid SewerGroupId { get; set; }
        public SewerGroupViewModel SewerGroup { get; set; }

        [Display(Name = "Tramo", Prompt = "Tramo")]
        public Guid SewerManifoldId { get; set; }
        public SewerManifoldViewModel SewerManifold { get; set; }
        public double TheoreticalLayer { get; set; }
        public double Filling { get; set; }
        [Display(Name = "Long. Excavada Acumulada")]
        public double ExcavatedLength { get; set; }
        [Display(Name = "Long. Instalada Acumulada")]
        public double InstalledLength { get; set; }
        [Display(Name = "Long. Rellenada Acumulada")]
        public double RefilledLength { get; set; }
        [Display(Name = "Long. Base Granular Acumulada")]
        public double GranularBaseLength { get; set; }
        public double Excavation { get; set; }
        public double Installation { get; set; }
        public double Filled { get; set; }
        public double FillLength { get; set; }
        public string Status { get; set; }
        public string ExcavatedLengthToExecute { get; set; }
        public string InstalledLengthToExecute { get; set; }
        public string RefilledLengthToExecute { get; set; }
        public string GranularBaseLengthToExecute { get; set; }
        public List<FoldingF7ViewModel> Folding { get; set; }
    }

}