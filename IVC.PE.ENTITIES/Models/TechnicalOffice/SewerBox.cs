using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Quality;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class SewerBox
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }
        public Project Project { get; set; }

        [Required]
        public string Code { get; set; }

        public double CoverLevel { get; set; }
        public double ArrivalLevel { get; set; }
        public double BottomLevel { get; set; }
        public double Height { get; set; }
        public int TerrainType { get; set; }
        public int SewerBoxType { get; set; }
        public double Thickness { get; set; }
        public double Diameter { get; set; }

        public int ProcessType { get; set; } //0: Projecto, 1:Replanteo, 2:Ejecución

        public int SewerOrderNumber { get; set; }
    }
}
