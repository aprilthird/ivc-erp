using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class CompactionDensityCertificateDetail
    {
        public Guid Id { get; set; }

        public DateTime TestDate { get; set; }

        public Guid CompactionDensityCertificateId { get; set; }

        public CompactionDensityCertificate CompactionDensityCertificate { get; set; }

        public Guid FillingLaboratoryTestId { get; set; }

        public FillingLaboratoryTest FillingLaboratoryTest { get; set; }

        public double WetDensity { get; set; }

        public double Moisture { get; set; }

        public double DryDensity { get; set; }

        public double DensityPercentage { get; set; }

        public int Layer { get; set; }

        public bool Latest { get; set; }
    }
}
