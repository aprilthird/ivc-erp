using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
   public class BiddingCurrencyType
    {
        public Guid Id { get; set; }

        public double Currency { get; set; }

        public DateTime PublicationDate { get; set; }
    }
}
