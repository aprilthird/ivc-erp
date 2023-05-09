using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class PreRequestFile
    {
        public Guid Id { get; set; }

        public Guid PreRequestId { get; set; }
        public PreRequest PreRequest { get; set; }

        public Uri FileUrl { get; set; }
    }
}
