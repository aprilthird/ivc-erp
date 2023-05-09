using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Quality
{
    [NotMapped]
    public class UspEquipmentCertificates
    {
        public Guid EquipmentCertificateId { get; set; }
        public Guid? ProjectId { get; set; }
        public Guid? EquipmentCertificateTypeId { get; set; }
        public string ProjectAbbreviation { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Serial { get; set; }
        public string EquipmentCertificateNumber { get; set; }
        public string EquipmentOwnerName { get; set; }
        public string EquipmentCertificateTypeName { get; set; }
        public string EquipmentCertifyingEntityName { get; set; }
        public string Observation { get; set; }
        public Guid? RenewalId { get; set; }
        public int RenewalOrder { get; set; }
        public DateTime? StartDate { get; set; }
        public string StartDateStr => StartDate.HasValue ? StartDate.Value.ToDateString() : string.Empty;
        public DateTime? EndDate { get; set; }
        public string EndDateStr => EndDate.HasValue ? EndDate.Value.ToDateString() : string.Empty;

        public DateTime? EntryDate { get; set; }

        public string EntryDateStr => EntryDate.HasValue ? EntryDate.Value.ToDateString() : string.Empty;
        public int? OperationalStatus { get; set; }
        public string OperationStatusDesc => OperationalStatus.HasValue ? ConstantHelpers.Quality.EquimentCertificate.OPERATIONAL_STATUS[OperationalStatus.Value] : string.Empty;
        public int? SituationStatus { get; set; }
        public string SituationStatusDesc => SituationStatus.HasValue ? ConstantHelpers.Quality.EquimentCertificate.SITUATIONAL_STATUS[SituationStatus.Value] : string.Empty;
        public string Operator { get; set; }
        public string PatternCalibrationRenewalReference { get; set; }
        public Uri FileUrl { get; set; }
        public int? Validity { get; set; }
        public bool HasAVoid { get; set; }
        public bool Days30 { get; set; }
        public bool Days15 { get; set; }
        //--
        public Guid? QualityFrontId { get; set; }
        public string Front { get; set; }

        public string Correlative { get; set; }

        public int InspectionType  { get; set;}
        public int CalibrationMethod { get; set;}
        public int CalibrationFrecuency { get; set;}

        public string InspectionTypeDesc => ConstantHelpers.Quality.EquimentCertificate.INSPECTION_TYPE[InspectionType];

        public string CalibrationMethodDesc => ConstantHelpers.Quality.EquimentCertificate.CALIBRATION_METHOD[CalibrationMethod];
   
        public string CalibrationFrecuencyDesc => ConstantHelpers.Quality.EquimentCertificate.CALIBRATION_FRECUENCY[CalibrationFrecuency];
    }
}
