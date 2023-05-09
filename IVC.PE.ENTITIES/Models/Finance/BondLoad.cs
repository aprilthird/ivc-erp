
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Finance
{
    public class BondLoad
    {

        public Guid Id { get; set; }

    /*   public Guid ProjectId { get; set; }

       public Project Project { get; set; }
       */
        public Guid BondGuarantorId { get; set; }
        public BondGuarantor BondGuarantor { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid BondTypeId { get; set; }
        public BondType BondType { get; set; }

        public Guid BankId { get; set; }
        public Bank Bank { get; set; }

        public string BondNumber { get; set; }

        public Guid BondRenovationId { get; set; }
        public BondRenovation BondRenovation { get; set; }
        public double PenAmmount { get; set; }
        public double UsdAmmount { get; set; }
        public int daysLimitTerm { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public string guaranteeDesc { get; set; }

       public Guid EmployeeId { get; set; }

       public Employee Employee { get; set; }
       

    }
}
