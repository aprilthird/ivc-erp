using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectPhaseViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BudgetTitleViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.SpecialityViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BlueprintViewModels
{
    public class BlueprintViewModel
    {
        public Guid? Id { get; set; }
        


        [Display(Name = "Lámina", Prompt = "Lámina")]
        public string Sheet { get; set; }

        [Display(Name = "Código de plano", Prompt = "Código de plano")]
        public string Description { get; set; }

        [Display(Name = "Presupuesto", Prompt = "Presupuesto")]
        public Guid BudgetTitleId { get; set; }

        public BudgetTitleViewModel BudgetTitle { get; set; }


        [Display(Name = "Tipo de plano", Prompt = "Tipo de plano")]
        public Guid? BlueprintTypeId { get; set; }

        public BlueprintTypeViewModel BlueprintType { get; set; }

        [Display(Name = "Fórmula", Prompt = "Fórmula")]
        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }
        [Display(Name = "Especialidad", Prompt = "Especialidad")]
        public Guid SpecialityId { get; set; }

        public SpecialityViewModel Speciality { get; set; }

        [Display(Name = "Frente", Prompt = "Frente")]
        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
        

        [Display(Name = "Fase", Prompt = "Fase")]
        public Guid? ProjectPhaseId { get; set; }

        public ProjectPhaseViewModel ProjectPhase { get; set; }

    }
}
