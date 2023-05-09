using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.Finances
{
    [NotMapped]
    public class UspBonds
    {
        public Guid BondAddId { get; set; }

        public Guid BondGuarantorId { get; set; }
        public Guid ProjectId { get; set; }
        public Guid BankId { get; set; }
        public string ProjectAbbreviation { get; set; }
        public string BondGuarantor { get; set; }
        public Guid BudgetTitleId { get; set; }
        public string BudgetTitle { get; set; }
        public Guid BondTypeId { get; set; }
        public string BondType { get; set; }
        public string Bank { get; set; }
        public string BondNumber { get; set; }
        public Guid BondRenovationId { get; set; }
        public int BondOrder { get; set; }
        public string BondName { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateDateString => $"{CreateDate.ToDateString()}";
        public DateTime EndDate { get; set; }
        public string EndDateString => $"{EndDate.ToDateString()}";
        public double PenAmmount { get; set; }
        public double IssueCostSum { get; set; }
        public string IssueCostSumFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", IssueCostSum);
        public string PenAmmountFormatted => String.Format(new CultureInfo("es-PE"), "{0:C}", PenAmmount);
        public string currencyType { get; set; }
        public string guaranteeDesc { get; set; }
        public int Validity { get; set; }
        public bool Days30 { get; set; }
        public bool Days15 { get; set; }
        public bool IsTheLast { get; set; }
        public Uri FileUrl { get; set; }
        public Uri IssueFileUrl { get; set; }
    }
}
