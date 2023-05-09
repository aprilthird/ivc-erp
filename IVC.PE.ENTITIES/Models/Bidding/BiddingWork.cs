using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Bidding
{
    public class BiddingWork
    {

        public Guid Id { get; set; }
        
        public Guid? BiddingCurrencyTypeId { get; set; }

        public BiddingCurrencyType BiddingCurrencyType { get; set; }
        
        public int CurrencyType { get; set; }

        public int CodeNumber { get; set; }

        public string Name { get; set; }

        public Guid BiddingWorkTypeId { get; set; }

        public BiddingWorkType BiddingWorkType { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int DifDate { get; set; }

        public DateTime? ReceivedDate { get; set; }

        public DateTime? LiquidationDate { get; set; }

        public Guid BusinessId { get; set; }

        public Business Business { get; set; }

        public Guid? BusinessParticipationFoldingId { get; set; }

        public BusinessParticipationFolding BusinessParticipationFolding { get; set; }
        public Uri ContractUrl { get; set; }

        public Uri ReceivedActUrl{get; set;}

        public Uri LiquidationUrl{get; set;}

        public Uri InVoiceUrl { get; set; }

        public Uri ConfirmedWork { get; set; }
        public double? LiquidationAmmount { get; set; }

        public double? ContractAmmount { get; set; }

        public double ParticipationAmmount { get; set; }

        public double? LiquidationDollarAmmount { get; set; }

        public double? ContractDollarAmmount { get; set; }

        public double? ParticipationDollarAmmount { get; set; }
    }
}
