using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerManifoldCostPerformance
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        public string Description { get; set; }

        public int TerrainType { get; set; }

        public double HeightMin { get; set; }
        public double HeightMax { get; set; }

        public string Unit { get; set; }

        public double Price { get; set; }

        public double Workforce { get; set; }
        public double Equipment { get; set; }
        public double Services { get; set; }
        public double Materials { get; set; }

        public double SecurityFactor { get; set; }
    }
}
