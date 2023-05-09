using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class RdpItemFootage
    {
        public Guid Id { get; set; }

        public Guid RdpItemId { get; set; }
        public RdpItem RdpItem { get; set; }

        public Guid RdpReportId { get; set; }
        public RdpReport RdpReport { get; set; }

        public decimal? PartialAmmount { get; set; }
        public decimal? AccumulatedAmmount { get; set; }
        public decimal? StakeOut { get; set; }
    }
}
