using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class FoldingFor05
    {
        public Guid Id { get; set; }
        public string LayerNumber { get; set; }
        public DateTime TestDate { get; set; }
        public Guid SewerManifoldFor05Id { get; set; }
        public SewerManifoldFor05 SewerManifoldFor05 { get; set; }
        public Guid FillingLaboratoryTestId { get; set; }
        public FillingLaboratoryTest FillingLaboratoryTest { get; set; }
        public double WetDensity { get; set; }
        public double MoisturePercentage { get; set; }
        public double DryDensity { get; set; }
        public double PercentageRequiredCompaction { get; set; }
        public double PercentageRealCompaction { get; set; }
        public string Status { get; set; }
    }
}
