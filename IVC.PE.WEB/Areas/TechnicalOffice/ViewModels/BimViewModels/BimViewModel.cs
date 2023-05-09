using IVC.PE.WEB.Areas.Admin.ViewModels.ProjectFormulaViewModels;
using IVC.PE.WEB.Areas.Admin.ViewModels.WorkFrontViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BimViewModels
{
    public class BimViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Formula", Prompt = "Formula")]
        public Guid ProjectFormulaId { get; set; }

        public ProjectFormulaViewModel ProjectFormula { get; set; }
        [Display(Name = "Frente", Prompt = "Frente")]
        public Guid WorkFrontId { get; set; }
        public WorkFrontViewModel WorkFront { get; set; }
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }
        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
    }
}
