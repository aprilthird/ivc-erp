using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class SewerManifoldFor05
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid SewerManifoldId { get; set; }
        public SewerManifold SewerManifold { get; set; }
        public string CertificateNumber { get; set; }
        public string LayerNumber { get; set; }
        public DateTime TestDate { get; set; } 
        public string Status { get; set; }
        public double Filling { get; set; }
        public int TheoreticalLayer { get; set; }
        public int LayersNumber { get; set; }
        public DateTime ShippingDate { get; set; }
        public Uri FileUrl { get; set; }
    }
}
