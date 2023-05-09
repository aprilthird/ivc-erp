using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class BiddingBusiness
    {
        public Guid Id { get; set; }

        public Guid? BusinessId { get; set; }

        public Business Business {get; set;}

        public string BiddingBusinessName { get; set; }

        public string BiddingBusinessRuc { get; set; }

        public DateTime BidddingBusinessCreationDate { get; set; }

        public string LegalRepresentant { get; set; }

        public Uri RucUrl { get; set; }

        public Uri TestimonyUrl { get; set; }


    }
}
