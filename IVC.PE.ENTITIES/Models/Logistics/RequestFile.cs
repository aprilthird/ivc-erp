using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class RequestFile
    {
        public Guid Id { get; set; }

        public Guid RequestId { get; set; }
        public Request Request { get; set; }

        public Uri FileUrl { get; set; }
    }
}
