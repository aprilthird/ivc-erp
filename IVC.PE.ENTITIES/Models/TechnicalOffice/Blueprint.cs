using IVC.PE.ENTITIES.Models.DocumentaryControl;
using IVC.PE.ENTITIES.Models.General;
using System;
using System.Collections.Generic;
using System.Text;

namespace IVC.PE.ENTITIES.Models.TechnicalOffice
{
    public class Blueprint
    {
        public Guid Id { get; set; }
        public Guid? ProjectId { get; set; }

        
        public Project Project { get; set; }

        public Guid? BlueprintTypeId { get; set; }

        public BlueprintType BlueprintType { get; set; }
        public Guid BudgetTitleId { get; set; }

        public BudgetTitle BudgetTitle { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public ProjectFormula ProjectFormula { get; set; }
        public Guid WorkFrontId { get; set; }
        public WorkFront WorkFront { get; set; }

        public Guid SpecialityId { get; set; }

        public Speciality Speciality { get; set; }

        public string Name { get; set; }

        public Guid? ProjectPhaseId { get; set; }
        public ProjectPhase ProjectPhase { get; set; }

        public string Sheet { get; set; }

        public string Description { get; set; }

    }
}
