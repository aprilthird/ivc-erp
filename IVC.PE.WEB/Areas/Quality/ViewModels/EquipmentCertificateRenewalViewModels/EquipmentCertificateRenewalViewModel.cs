using IVC.PE.CORE.Helpers;
using IVC.PE.WEB.Areas.Admin.ViewModels.SewerGroupViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.PaternCalibrationRenewalViewModels;
using IVC.PE.WEB.Areas.Quality.ViewModels.QualityFrontViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IVC.PE.WEB.Areas.Quality.ViewModels.EquipmentCertificateRenewalViewModels
{
    public class EquipmentCertificateRenewalViewModel
    {
        public Guid? Id { get; set; }

        [Display(Name = "Observación", Prompt = "Observación")]
        public string Observation { get; set; }

        public int RenewalOrder { get; set; }

        [Display(Name = "# de certificado", Prompt = "# de certificado")]
        public string EquipmentCertificateNumber { get; set; }
        public Guid EquipmentCertificateId { get; set; }
        public EquipmentCertificateViewModel EquipmentCertificate { get; set; }

        //[Display(Name = "Entidad Certificadora", Prompt = "Entidad Certificadora")]
        //public string CertifyingEntity { get; set; }
        [Display(Name = "Inicio de vigencia", Prompt = "Inicio de vigencia")]
        public string StartDate { get; set; } = DateTime.UtcNow.Date.ToShortDateString();
        [Display(Name = "Fin de vigencia", Prompt = "Fin de vigencia")]
        public string EndDate { get; set; }
        [Display(Name = "Condición", Prompt = "Condición")]
        public int OperationalStatus { get; set; }
        [Display(Name = "Condición", Prompt = "Condición")]
        public string OperationalStatusDesc => ConstantHelpers.Quality.EquimentCertificate.OPERATIONAL_STATUS[OperationalStatus];
        [Display(Name = "Situación", Prompt = "Situación")]
        public int SituationStatus { get; set; }
        [Display(Name = "Situación", Prompt = "Situación")]
        public string SituationStatusDesc => ConstantHelpers.Quality.EquimentCertificate.SITUATIONAL_STATUS[SituationStatus];
        [Display(Name = "Entidad Certificadora", Prompt = "Entidad Certificadora")]
        public Guid? EquipmentCertifyingEntityId { get; set; }
        public EquipmentCertifyingEntityViewModel EquipmentCertifyingEntity { get; set; }

        [Display(Name = "# de Referencia de Patron de calibración", Prompt = "# de Referencia de Patron de calibración")]
        public Guid? PatternCalibrationRenewalId { get; set; }

        public PatternCalibrationRenewalViewModel PatternCalibrationRenewal { get; set; }

        [Display(Name = "Operador", Prompt = "Operador")]
        public Guid? EquipmentCertificateUserOperatorId { get; set; }
        public EquipmentCertificateUserOperatorViewModel EquipmentCertificateUserOperator { get; set; }

        public IEnumerable<string> Responsibles { get; set; }
        [Display(Name = "URL", Prompt = "URL")]
        public Uri FileUrl { get; set; }

        [DataType(DataType.Upload)]
        [Display(Name = "Archivo", Prompt = "Archivo")]
        public IFormFile File { get; set; }

        //Id del Certificado pasado, y que vamos a renovar
        public Guid? RenewalId { get; set; }

        public bool HasAVoid { get; set; }
        //--
        [Display(Name = "Frente", Prompt = "Frente")]
        public Guid? QualityFrontId { get; set; }

        public QualityFrontViewModel QualityFront { get; set; }
        [Display(Name = "Tipo de Inspección", Prompt = "Tipo de Inspección")]
        public int InspectionType { get; set; }
        public string InspectionTypeDesc => ConstantHelpers.Quality.EquimentCertificate.INSPECTION_TYPE[InspectionType];
        [Display(Name = "Método de Calibración", Prompt = "Método de Calibración")]
        public int CalibrationMethod { get; set; }
        public string CalibrationMethodDesc => ConstantHelpers.Quality.EquimentCertificate.CALIBRATION_METHOD[CalibrationMethod];
        [Display(Name = "Frecuencia de Calibración", Prompt = "Frecuencia de Calibración")]
        public int CalibrationFrecuency { get; set; }
        public string CalibrationFrecuencyDesc => ConstantHelpers.Quality.EquimentCertificate.CALIBRATION_FRECUENCY[CalibrationFrecuency];
    }
}
