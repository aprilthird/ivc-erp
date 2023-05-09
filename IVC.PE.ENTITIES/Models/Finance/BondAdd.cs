
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
namespace IVC.PE.ENTITIES.Models.Finance
{
    public class BondAdd
    {

        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }


        public Guid BondGuarantorId { get; set; }
        public BondGuarantor BondGuarantor { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid BondTypeId { get; set; }
        public BondType BondType { get; set; }

        public Guid BankId { get; set; }
        public Bank Bank { get; set; }

        public string BondNumber { get; set; }

        public int NumberOfRenovations { get; set; }


        //public Guid EmployeeId { get; set; }
        //public Employee Employee { get; set; }

        //public IEnumerable<BondFile> BondFiles { get; set; }

        //public bool Days30 { get; set; } = false;

        //public bool Days15 { get; set; } = false;


        //public Guid BondRenovationId { get; set; }
        //public BondRenovation BondRenovation { get; set; }
        //public string guaranteeDesc { get; set; }

        /*public double PenAmmount { get; set; }
        public double UsdAmmount { get; set; }
        public string currencyType { get; set; }
        public int daysLimitTerm { get; set; }
        public DateTime CreateDate { get; set; } = DateTime.UtcNow;
        public DateTime EndDate { get; set; } = DateTime.UtcNow;*/

        /*[NotMapped]
        public string DateInitial => $"{CreateDate.Date}";

        [NotMapped]
        public string DateEnd => $"{CreateDate.AddDays(daysLimitTerm).Date}";

        [NotMapped]
        public string CreateDateOnlyDate => (CreateDate.Date).ToString();

        [NotMapped]
        public string EndDateOnlyDate => (EndDate.Date).ToString();



        [NotMapped]
        public double daysToEnd => (CreateDate.AddDays(daysLimitTerm) - DateTime.UtcNow).TotalDays;

        [NotMapped]
        public string daysLimit => (EndDate.Date - DateTime.UtcNow).TotalDays.ToString();

        [NotMapped]
        public string penAmmount2 => String.Format("{0:C}", PenAmmount);

        [NotMapped]
        public string DaysToEnd => $"{DateTime.Now.Date}";

        [NotMapped]
        public bool IsEnabled => Convert.ToInt32(daysToEnd) >= 0;


        [NotMapped]
        public bool IsExpired => Convert.ToInt32(daysToEnd) < 0;


        [NotMapped]
        public bool isClosed => daysLimitTerm == 1;

        [NotMapped]
        public int daysToEnd2 { get; set; }*/



    }
}
