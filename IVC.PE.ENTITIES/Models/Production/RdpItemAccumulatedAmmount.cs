using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class RdpItemAccumulatedAmmount
    {
        public Guid Id { get; set; }

        public Guid RdpItemId { get; set; }
        public RdpItem RdpItem { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public decimal? AccumulatedAmmount { get; set; }
    }
}
