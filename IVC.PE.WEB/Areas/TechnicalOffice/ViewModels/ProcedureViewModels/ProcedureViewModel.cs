using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.DocumentTypeViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProcessViewModels;
using IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.TechnicalVersionViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.TechnicalOffice.ViewModels.ProcedureViewModels
{
    public class ProcedureViewModel
    {
        public Guid? Id { get; set; }
        [Display(Name = "Nombre", Prompt = "Nombre")]
        public string Name { get; set; }

        [Display(Name = "Código", Prompt = "Código")]
        public string Code { get; set; }
        [Display(Name = "Tipo de Documento", Prompt = "Tipo de Documento")]
        public Guid? DocumentTypeId { get; set; }
        public DocumentTypeViewModel DocumentType { get; set; }


        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        public Uri FileUrl { get; set; }

        [Display(Name = "Archivo Excel", Prompt = "Archivo Excel")]
        public IFormFile File2 { get; set; }
        public Uri FileUrl2 { get; set; }

        [Display(Name = "Procesos", Prompt = "Procesos")]

        public IEnumerable<Guid> Processes { get; set; }
    }

    public class ProcedureFilesViewModel
    {
        [DataType(DataType.Upload)]
        [Display(Name = "Adjuntar archivos", Prompt = "Adjuntar archivos")]
        public IEnumerable<IFormFile> ProcedureFiles { get; set; }

    }
}
