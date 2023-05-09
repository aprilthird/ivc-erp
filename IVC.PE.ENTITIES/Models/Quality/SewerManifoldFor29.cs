using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class SewerManifoldFor29
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid SewerManifoldId { get; set; }
        public SewerManifold SewerManifold { get; set; }
        public string For01ProtocolNumber { get; set; }
        public DateTime AsphaltDate { get; set; }
        public int AsphaltType { get; set; }
        public int Thickness { get; set; }
        public double AsphaltArea { get; set; }
        public double AreaToValue { get; set; }
        public double Pavement2InReview { get; set; }
        public double Pavement3InReview { get; set; }
        public double Pavement3InMixedReview { get; set; }
        public Uri FileUrl { get; set; }
    }
}
