using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.HumanResources.ViewModels.WorkerMedicalRestViewModels
{
    public class WorkerMedicalRestViewModel
    {
        public Guid? Id { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Trabajador", Prompt = "Trabajador")]
        public Guid WorkerId { get; set; }
        public WorkerViewModel Worker { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Tipo de Documento", Prompt = "Tipo de Documento")]
        public int FileType { get; set; }
        public string FileTypeDescription => ConstantHelpers.WorkerMedicalRest.FileType.VALUES[FileType];
        public Uri FileUrl { get; set; }
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Fecha de Inicio", Prompt = "Fecha de Inicio")]
        public String InitDate { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Duración días", Prompt = "Duración días")]
        public int DurationDays { get; set; }
        public String EndDate { get; set; }
    }
}
