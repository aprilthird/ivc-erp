using IVC.PE.CORE.Helpers;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.ConcreteQualityCertificateViewModels
{
    public class ConcreteQualityCertificateViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "N° de Registro", Prompt = "N° de Registro")]
        public string CertificateSerialNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "N° de Registro de Vaciado", Prompt = "N° de Registro de Vaciado")]
        public string For07SerialNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Fecha de Muestreo", Prompt = "Fecha de Muestreo")]
        public string SamplingDate { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Fecha de Ensayo", Prompt = "Fecha de Ensayo")]
        public string TestDate { get; set; }

        [Display(Name = "Edad", Prompt = "Edad")]
        public int Age { get; set; }

        public int RealAge { get; set; }

        [Display(Name = "Probeta N° 1", Prompt = "Probeta N° 1")]
        public double FirstResult { get; set; }

        [Display(Name = "Probeta N° 2", Prompt = "Probeta N° 2")]
        public double SecondResult { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Archivo Certificado", Prompt = "Archivo Certificado")]
        public IFormFile CertificateFile { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Archivo For 10", Prompt = "Archivo For 10")]
        public IFormFile For07File { get; set; }

        public Uri CertificateFileUrl { get; set; }

        public Uri For07FileUrl { get; set; }

        public IList<ConcreteQualityCertificateDetailViewModel> Details { get; set; }
    }
}
