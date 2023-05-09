using IVC.PE.CORE.Helpers;
using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Logistics
{
    public class PreRequest
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public int CorrelativeCode { get; set; }

        public string CorrelativePrefix { get; set; }

        public int OrderStatus { get; set; } = ConstantHelpers.Logistics.RequestOrder.Status.PRE_ISSUED;

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        //public Guid? SupplyFamilyId { get; set; }

        //public SupplyFamily SupplyFamily { get; set; }

        public Guid? ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public int RequestType { get; set; }

        public DateTime? IssueDate { get; set; }

        public DateTime? DeliveryDate { get; set; }

        public string Observations { get; set; }

        public int AttentionStatus { get; set; } = ConstantHelpers.Logistics.RequestOrder.AttentionStatus.PENDING;

        public string IssuedUserId { get; set; }

        //public IEnumerable<PreRequestUser> PreRequestUsers { get; set; }

        public IEnumerable<PreRequestItem> PreRequestItems { get; set; }
    }
}
