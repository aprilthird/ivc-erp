using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Quality
{
    public class SewerManifoldFor47
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Project Project { get; set; }
        public Guid SewerManifoldId { get; set; }
        public SewerManifold SewerManifold { get; set; }

        public double LengthOfDiggingN { get; set; }
        public double LengthOfDiggingSR { get; set; }
        public double LengthOfDiggingR { get; set; }

        public int WorkBookNumber { get; set; }
        public int WorkBookSeat { get; set; }
        public DateTime WorkBookRegistryDate { get; set; }
        public Uri FileUrl { get; set; }
        public string For01ProtocolNumber { get; set; }
        public int BZiRealTerrainType { get; set; }
        public int BZjRealTerrainType { get; set; }
    }
}
