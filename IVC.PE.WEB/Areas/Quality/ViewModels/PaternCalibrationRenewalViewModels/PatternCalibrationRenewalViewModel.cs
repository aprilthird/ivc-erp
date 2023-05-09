using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.PatternCalibrationViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.PaternCalibrationRenewalViewModels
{
    public class PatternCalibrationRenewalViewModel
    {
        public Guid? Id { get; set; }

        public int RenewalOrder { get; set; }

        public Guid PatternCalibrationId { get; set; }
        public PatternCalibrationViewModel PatternCalibration { get; set; }

        [Display(Name = "# de Referencia", Prompt = "# de Referencia")]
        public string ReferenceNumber { get; set; }

        [Display(Name = "Inicio de vigencia", Prompt = "Inicio de vigencia")]
        public string CreateDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Fin de vigencia", Prompt = "Fin de vigencia")]
        public string EndDate { get; set; }

        [Display(Name = "Entidad Certificadora", Prompt = "Entidad Certificadora")]
        public Guid? EquipmentCertifyingEntityId { get; set; }

        public EquipmentCertifyingEntityViewModel EquipmentCertifyingEntity { get; internal set; }
        [Display(Name = "Solicitante", Prompt = "Solicitante")]
        public string Requestioner { get; set; }

        [Display(Name = "URL", Prompt = "URL")]
        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        //Id del Certificado pasado, y que vamos a renovar
        public Guid? RenewalId { get; set; }

        public IEnumerable<string> Responsibles { get; set; }
    }
}
