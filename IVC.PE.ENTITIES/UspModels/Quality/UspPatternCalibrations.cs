using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Quality
{
    [NotMapped]
    public class UspPatternCalibrations
    {
        public Guid PatternCalibrationId { get; set; }

        public Guid? ProjectId { get; set; }
        public string ProjectAbbreviation { get; set; }

        public Guid? RenewalId { get; set; }

        public DateTime? CreateDate { get; set; }
        public string CreateDateStr => CreateDate.HasValue ? CreateDate.Value.ToDateString() : string.Empty;
        public DateTime? EndDate { get; set; }
        public string EndDateStr => EndDate.HasValue ? EndDate.Value.ToDateString() : string.Empty;

        public string EquipmentCertifyingEntityName { get; set; }

        public string Name { get; set; }

        public string Requestioner { get; set; }

        public string ReferenceNumber { get; set; }

        public Uri FileUrl { get; set; }

        public int? Validity { get; set; }

        public bool Days30 { get; set; }
        public bool Days15 { get; set; }

        public int RenewalOrder { get; set; }
    }
}
