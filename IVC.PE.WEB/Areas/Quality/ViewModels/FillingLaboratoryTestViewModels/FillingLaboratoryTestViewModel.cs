using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.CertificateIssuerViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.FillingLaboratoryTestViewModels
{
    public class FillingLaboratoryTestViewModel
    {
        public Guid? Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "N° Certificado", Prompt = "N° Certificado")]
        public string SerialNumber { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = ConstantHelpers.Messages.Validation.REQUIRED)]
        [Display(Name = "Muestra N°", Prompt = "Muestra N°")]
        public string RecordNumber { get; set; }
        
        [Display(Name = "Fecha de Muestreo", Prompt = "Fecha de Muestreo")]
        public string SamplingDate { get; set; }

        [Display(Name = "Fecha de Ensayo", Prompt = "Fecha de Ensayo")]
        public string TestDate { get; set; }
        
        [Display(Name = "Clase Material", Prompt = "Clase Material")]
        public int MaterialType { get; set; }
        
        [Display(Name = "Procedencia", Prompt = "Procedencia")]
        public int OriginType { get; set; }

        [Display(Name = "Certificado por", Prompt = "Certificado por")]
        public Guid CertificateIssuerId { get; set; }

        public CertificateIssuerViewModel CertificateIssuer { get; set; }

        [Display(Name = "Ubic. de Muestreo", Prompt = "Ubic. de Muestreo")]
        public string Ubication { get; set; }
        
        [Display(Name = "Cont. Humedad Material (%)", Prompt = "Cont. Humedad Material")]
        public double MaterialMoisture { get; set; }

        [Display(Name = "Optimo C. Humedad corregido (%)", Prompt = "Optimo C. Humedad")]
        public double OptimumMoisture { get; set; }
        
        [Display(Name = "Max. Densidad seca corregida (gr/cm3)", Prompt = "Max. Densidad")]
        public double MaxDensity { get; set; }
        
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        public Uri FileUrl { get; set; }
        public Guid OriginTypeFillingLaboratoryId { get; set; }
        public OriginTypeFillingLaboratoryViewModel OriginTypeFillingLaboratory { get; set; }
    }
}
