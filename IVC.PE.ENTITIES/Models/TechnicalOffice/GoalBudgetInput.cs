using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.Logistics;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class GoalBudgetInput
    {
        public Guid Id { get; set; }

        public int OrderNumber { get; set; }

        public Guid WorkFrontId { get; set; }

        public WorkFront WorkFront { get; set; }

        public Guid MeasurementUnitId { get; set; }

        public MeasurementUnit MeasurementUnit { get; set; }

        public Guid SupplyId { get; set; }

        public Supply Supply { get; set; }

        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }

        public double Metered { get; set; }

        public double CurrentMetered { get; set; }

        public double WarehouseCurrentMetered { get; set; }

        public double WarehouseAccumulatedMetered { get; set; }

        public double UnitPrice { get; set; }

        public double Parcial { get; set; }
    }
}
