using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class SewerManifoldFor37A
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid SewerManifoldId { get; set; }
        public SewerManifold SewerManifold { get; set; }
        public string For01ProtocolNumber { get; set; }
        public int HotMeltsNumber { get; set; }
        public int ElectrofusionsNumber { get; set; }
        public int ElectrofusionsPasNumber { get; set; }
        public DateTime StartElectrofusionDate { get; set; }
        public DateTime EndElectrofusionDate { get; set; }
        public string FirstPipeBatch { get; set; }
        public string SecondPipeBatch { get; set; }
        public string ThridPipeBatch { get; set; }
        public string ForthPipeBatch { get; set; }
        public Uri FileUrl { get; set; }

    }
}
