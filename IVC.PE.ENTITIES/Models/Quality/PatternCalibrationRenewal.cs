using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class PatternCalibrationRenewal
    {
        public Guid Id { get; set; }
        public Guid PatternCalibrationId { get; set; }
        public PatternCalibration PatternCalibration { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public string ReferenceNumber { get; set; }
        public Guid? EquipmentCertifyingEntityId { get; set; }
        public EquipmentCertifyingEntity EquipmentCertifyingEntity { get; set; }
        public string Requestioner { get; set; }
        public Uri FileUrl { get; set; }
        public int RenewalOrder { get; set; }

        public bool Days30 { get; set; } = false;

        public bool Days15 { get; set; } = false;
    }
}
