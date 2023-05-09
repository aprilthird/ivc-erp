using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class EquipmentCertificateRenewal
    {
        public Guid Id { get; set; }

        public Guid EquipmentCertificateId { get; set; }
        public EquipmentCertificate EquipmentCertificate { get; set; }

        public Guid? EquipmentCertifyingEntityId { get; set; }
        public EquipmentCertifyingEntity EquipmentCertifyingEntity { get; set; }

        public Guid? PatternCalibrationRenewalId { get; set; }
        public PatternCalibrationRenewal PatternCalibrationRenewal { get; set; }

        public Guid? EquipmentCertificateUserOperatorId { get; set; }
        public EquipmentCertificateUserOperator EquipmentCertificateUserOperator { get; set; }

        public DateTime StartDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;

        public string EquipmentCertificateNumber { get; set; }

        public int OperationalStatus { get; set; }
        public int SituationStatus { get; set; }

        public string Observation { get; set; }

        public Uri FileUrl { get; set; }

        public int RenewalOrder { get; set; }

        public bool HasAVoid { get; set; }

        public Guid? QualityFrontId { get; set; }

        public QualityFront QualiyFront { get; set; }
        public bool Days30 { get; set; } = false;

        public bool Days15 { get; set; } = false;

        public int InspectionType { get; set; }

        public int CalibrationMethod { get; set; }

        public int CalibrationFrecuency { get; set; }


    }
}
