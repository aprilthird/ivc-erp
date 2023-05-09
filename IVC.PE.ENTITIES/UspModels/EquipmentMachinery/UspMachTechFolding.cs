using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspMachTechFolding
    {
        public string Center { get; set; }
        public string Tradename { get; set; }
        public Guid FoldingId { get; set; }
        public DateTime EndDateTechnicalRevision { get; set; }
        public string EndDateTechnicalRevisionString => EndDateTechnicalRevision.ToDateString();
        public DateTime StartDateTechnicalRevision { get; set; }
        public string StartDateTechnicalRevisionString => StartDateTechnicalRevision.ToDateString();

        public Uri TechnicalRevisionFileUrl { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }
        public string SerieNumber { get; set; }
        public int Validity { get; set; }

        public bool Days30 { get; set; }
        public bool Days15 { get; set; }

        public int TechnicalOrder { get; set; }
    }
}
