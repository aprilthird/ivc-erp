using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Finance
{
    public class BondFile
    {
        public Guid Id { get; set; }

        public int Type { get; set; }

        public Uri FileUrl { get; set; }

        //public Guid BondAddId { get; set; }

        //public BondAdd BondAdd { get; set; }

        public Guid BondRenovationId { get; set; }
        public BondRenovation BondRenovation { get; set; }

    }
}
