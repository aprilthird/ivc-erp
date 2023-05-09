using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.Aggregation;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class CompactionDensityCertificate
    {
        public Guid Id { get; set; }

        [Required]
        public string SerialNumber { get; set; }
        
        public DateTime ExecutionDate { get; set; }

        public Guid SewerLineId { get; set; }

        public SewerLine SewerLine { get; set; }

        public int MaterialType { get; set; } = ConstantHelpers.Certificate.FillingLaboratory.MaterialType.OWN_FILLING;

        public Quarry Quarry { get; set; }

        public Guid QuarryId { get; set; }

        public Uri FileUrl { get; set; }

        public ICollection<CompactionDensityCertificateDetail> CompactionDensityCertificateDetails { get; set; }
    }
}
