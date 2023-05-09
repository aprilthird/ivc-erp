using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class FoldingMeteredsRestatedByPartida
    {
        public Guid Id { get; set; }

        public Guid MeteredsRestatedByPartidaId { get; set; }

        public MeteredsRestatedByPartida MeteredsRestatedByPartida { get; set; }

        public Guid SewerGroupId { get; set; }

        public SewerGroup SewerGroup { get; set; }

        public double Metered { get; set; }

        public double Amount { get; set; }
    }
}
