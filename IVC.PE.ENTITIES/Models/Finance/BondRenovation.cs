using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Finance
{
    public class BondRenovation
    {
        public Guid Id { get; set; }

        public int BondOrder { get; set; }

        public string BondName { get; set; }

        public Guid BondAddId { get; set; }
        public BondAdd BondAdd { get; set; }

        public double PenAmmount { get; set; }
        public double UsdAmmount { get; set; }

        public double IssueCost { get; set; }
        public double IssueCostUsd { get; set; }

        public string currencyType { get; set; }
        public int daysLimitTerm { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;
        public string guaranteeDesc { get; set; }

        public bool Days30 { get; set; } = false;

        public bool Days15 { get; set; } = false;

        public Uri FileUrl { get; set; }

        public Uri IssueFileUrl { get; set; }

        public bool IsTheLast { get; set; }
    }
}
