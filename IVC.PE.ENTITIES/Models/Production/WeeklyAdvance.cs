using IVC.PE.ENTITIES.Models.General;
using IVC.PE.ENTITIES.Models.HumanResources;
using IVC.PE.ENTITIES.Models.TechnicalOffice;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.Production
{
    public class WeeklyAdvance
    {
        public Guid Id { get; set; }

        public Guid ProjectFormulaId { get; set; }
        public ProjectFormula ProjectFormula { get; set; }

        public Guid ProjectCalendarWeekId { get; set; }
        public ProjectCalendarWeek ProjectCalendarWeek { get; set; }

        public Guid WorkFrontHeadId { get; set; }
        public WorkFrontHead WorkFrontHead { get; set; }

        public Guid SewerGroupId { get; set; }
        public SewerGroup SewerGroup { get; set; }

        public double TotalNetBudget { get; set; }

        public double AccumulatedBudget { get; set; }

        public double PercentageAdvance { get; set; }

        public int WorkersNumberOP { get; set; }

        public int WorkersNumberOF { get; set; }

        public int WorkersNumberPE { get; set; }

        public int WorkerNumberTotal { get; set; }

        public double SaleMO { get; set; }

        public double SaleEQ { get; set; }

        public double SaleSubcontract { get; set; }

        public double SaleMaterials { get; set; }

        public double SaleTotal { get; set; }

        public double GoalMO { get; set; }

        public double GoalEQ { get; set; }

        public double GoalSubcontract { get; set; }

        public double GoalMaterials { get; set; }

        public double GoalTotal { get; set; }

        public double CostMO { get; set; }

        public double CostEQ { get; set; }

        public double CostSubcontract { get; set; }

        public double CostMaterials { get; set; }

        public double CostTotal { get; set; }

        public double MarginActual { get; set; }

        public double MarginAccumulated { get; set; }
    
    }
}
