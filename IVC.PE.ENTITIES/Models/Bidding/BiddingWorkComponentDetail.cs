using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
   public class BiddingWorkComponentDetail
    {
        public Guid Id { get; set; }

        public Guid BiddingWorkId { get; set; }

        public BiddingWork BiddingWork { get; set; }

        public Guid BiddingWorkComponentId { get; set; }

        public BiddingWorkComponent BiddingWorkComponent { get; set; }
    }
}
