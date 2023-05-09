using IVC.PE.ENTITIES.Models.TechnicalOffice;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.LetterViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.BlueprintViewModels
{
    public class BlueprintFoldingViewModel
    {
        public Guid? Id { get; set; }

        public Guid BlueprintId { get; set; }
        public BlueprintViewModel Blueprint { get; set; }

        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }

        public string TempVersion { get; set; }

        [Display(Name = "Versión", Prompt = "Versión")]
        public Guid TechnicalVersionId { get; set; }

        public TechnicalVersionViewModel TechnicalVersion { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        public Uri FileUrl { get; set; }

        [Display(Name = "CAD", Prompt = "CAD")]
        public IFormFile Cad { get; set; }
        public Uri CadUrl { get; set; }

        [Display(Name = "Carta de Aprobación", Prompt = "Carta de Aprobación")]
        public Guid? LetterId { get; set; }

        public LetterViewModel Letter { get; set; }

        [Display(Name = "Fecha de aprobaciòn", Prompt = "Fecha de aprobaciòn")]
        public string BlueprintDateStr { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
    }
}
