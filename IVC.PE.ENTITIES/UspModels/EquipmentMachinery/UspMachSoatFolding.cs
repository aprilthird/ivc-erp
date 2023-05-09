using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace IVC.PE.ENTITIES.UspModels.EquipmentMachinery
{
    [NotMapped]
    public class UspMachSoatFolding
    {
        public string Center { get; set; }
        public string Tradename { get; set; }
        public Guid FoldingId { get; set; }
        public DateTime EndDateSOAT { get; set; }
        public string EndDateSOATString => EndDateSOAT.ToDateString();
        public DateTime StartDateSOAT { get; set; }
        public string StartDateSOATString => StartDateSOAT.ToDateString();

        public Uri SOATFileUrl { get; set; }

        public string Model { get; set; }

        public string Brand { get; set; }

        public string SerieNumber { get; set; }

        public int Validity { get; set; }

        public bool Days30 { get; set; }
        public bool Days15 { get; set; }

        public int SoatOrder { get; set; }
    }
}
