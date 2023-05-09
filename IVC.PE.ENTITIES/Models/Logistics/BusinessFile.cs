using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class BusinessFile
    {
        public Guid Id { get; set; }

        public int Type { get; set; }

        public Uri FileUrl { get; set; }

        public Guid BusinessId { get; set; }

        public Business Business { get; set; }
    }
}
