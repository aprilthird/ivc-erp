using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class ProfessionalExperienceFolding
    {
        public Guid Id { get; set; }

        public Guid ProfessionalId { get; set; }

        public Professional Professional { get; set; }

        public string Number { get; set; }

        public Guid BusinessId { get; set; }

        public Business Business { get; set; }

        public Guid BiddingWorkId {get; set;}

        public BiddingWork BiddingWork { get; set; }

        public Guid PositionId { get; set; }

        public Position Position { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int Dif { get; set; }
        public string Observations { get; set; }

        public Uri FileUrl { get; set; }

        public int Order { get; set; }
    }
}
