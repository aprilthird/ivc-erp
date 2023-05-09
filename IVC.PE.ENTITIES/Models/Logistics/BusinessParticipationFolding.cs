using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class BusinessParticipationFolding
    {
        public Guid Id { get; set; }

        public Guid BusinessId { get; set; }
        public Business Business { get; set; }

        public string Name { get; set; }
        public double IvcParticipation { get; set; }

        public Uri TestimonyUrl { get; set; }

        public int Order { get; set; }
    }
}
