using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.WorkbookViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.DocumentaryControl.ViewModels.WorkbookSeatViewModels
{
    public class WorkbookSeatViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Número", Prompt = "Número")]
        public int Number { get; set; }

        [Display(Name = "Cuaderno de Obra", Prompt = "Cuaderno de Obra")]
        public Guid WorkbookId { get; set; }

        public WorkbookViewModel Workbook { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Escribe", Prompt = "Escribe")]
        public int WroteBy { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Fecha", Prompt = "Fecha")]
        public string Date { get; set; }

        [Display(Name = "Fecha de Respuesta", Prompt = "Fecha de Respuesta")]
        public string ResponseDate { get; set; }

        [Display(Name = "Período de Respuesta", Prompt = "Período de Respuesta")]
        public int? ResponseTermDays { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Asunto", Prompt = "Asunto")]
        public string Subject { get; set; }

        [Display(Name = "Tipo", Prompt = "Tipo")]
        public int Type { get; set; }

        [Display(Name = "Tipo", Prompt = "Tipo")]
        public Guid? WorkbookTypeId { get; set; }

        public WorkbookTypeViewModel WorkbookType { get; set;}

        [Display(Name = "Estado", Prompt = "Estado")]
        public int Status { get; set; }

        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        public Uri FileUrl { get; set; }
        [Display(Name = "Detalle", Prompt = "Detalle")]
        public string Detail { get; set; }
    }
}
