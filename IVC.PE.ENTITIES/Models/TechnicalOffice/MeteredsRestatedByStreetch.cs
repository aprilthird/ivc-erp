using System;
using System.Collections.Generic;
using System.Text;
using IVC.PE.ENTITIES.Models.General;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class MeteredsRestatedByStreetch
    {
        public Guid Id { get; set; }
        public Guid BudgetTitleId { get; set; }
        public BudgetTitle BudgetTitle { get; set; }
        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }
        public string ItemNumber { get; set; }
        public string Description { get; set; }
        public string Unit { get; set; }
        public double Metered { get; set; }
        public Guid? WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }
        public Guid? SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public Guid ProjectId { get;set; }
        public Project Project { get; set; }
        
    }
}
