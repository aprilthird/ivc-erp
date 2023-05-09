using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using static IVC.PE.CORE.Helpers.ConstantHelpers.Certificate.FillingLaboratory;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class FillingLaboratoryTest
    {
        public Guid Id { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        [Required]
        public string RecordNumber { get; set; }

        public DateTime TestDate { get; set; }

        public DateTime SamplingDate { get; set; }

        public int MaterialType { get; set; } = ConstantHelpers.Certificate.FillingLaboratory.MaterialType.OWN_FILLING;

        public int OriginType { get; set; } = ConstantHelpers.Certificate.FillingLaboratory.OriginType.OWN_EXCAVATION;
        public Guid OriginTypeFillingLaboratoryId { get; set; }
        public OriginTypeFillingLaboratory OriginTypeFillingLaboratory { get; set; }

        public Guid CertificateIssuerId { get; set; }

        public CertificateIssuer CertificateIssuer { get; set; }
        
        [Required]
        public string Ubication { get; set; }

        public double MaterialMoisture { get; set; }

        public double OptimumMoisture { get; set; }

        public double MaxDensity { get; set; }

        public Uri FileUrl { get; set; }
    }
}
