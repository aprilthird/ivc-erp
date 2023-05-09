using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class BudgetInput
    {
        public Guid Id { get; set; }

        [Required]
        public string Code { get; set; }

        public string Description { get; set; }

        [NotMapped]
        public string FullName => $"{Code} - {Description}";

        public Guid ProjectId { get; set; }

        public Project Project { get; set; }

        public Guid MeasurementUnitId { get; set; }

        public MeasurementUnit MeasurementUnit { get; set; }

        public Guid SupplyFamilyId { get; set; }

        public SupplyFamily SupplyFamily { get; set; }

        public Guid? SupplyGroupId { get; set; }

        public SupplyGroup SupplyGroup { get; set; }

        public double SaleUnitPrice { get; set; }
        
        public double GoalUnitPrice { get; set; }

        public int Group { get; set; }

        public Guid? BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid? ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public Guid? BudgetFormulaId { get; set; }

        public BudgetFormula BudgetFormula { get; set; }

        public Guid? BudgetTypeId { get; set; }

        public BudgetType BudgetType { get; set; }

        public double Metered { get; set; }

        public double Parcial { get; set; }

        public IEnumerable<BudgetInputAllocation> BudgetInputAllocations { get; set; }

        public IEnumerable<BudgetInputAllocationGroup> BudgetInputAllocationGroups { get; set; }

        public IEnumerable<RequestItem> RequestItems { get; set; }

    }
}
