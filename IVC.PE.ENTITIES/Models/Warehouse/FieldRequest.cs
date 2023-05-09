using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Warehouse
{
    public class FieldRequest
    {
        public Guid Id { get; set; }

        public int DocumentNumber { get; set; }

        public Guid BudgetTitleId { get; set; }
        public BudgetTitle BudgetTitle { get; set; }
        /*
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }
        */

        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        //public Guid WarehouseId { get; set; }
        //public Warehouse Warehouse { get; set; }

        //public Guid SupplyFamilyId { get; set; }
        //public SupplyFamily SupplyFamily { get; set; }

        public DateTime DeliveryDate { get; set; }

        public DateTime? IssueDate { get; set; }

        public int Status { get; set; }

        public string Observation { get; set; }

        public string IssuedUserId { get; set; }

        public string WorkOrder { get; set; }

        public Uri FileUrl { get; set; }

        public IEnumerable<FieldRequestFolding> FieldRequestFolding { get; set; }

        public IEnumerable<FieldRequestProjectFormula> FieldRequestProjectFormulas { get; set; }
    }
}
