using IVC.PE.CORE.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace IVC.PE.ENTITIES.UspModels.TechnicalOffice
{
    [NotMapped]
    public class UspBlueprint
    {
        public Guid? Id { get; set; }

        public Guid ProjectId { get; set; }

        public string Name { get; set; }

        public Guid ProjectFormulaId { get; set; }

        public string ProjectFormulaCode { get; set; }
        public string ProjectFormulaName { get; set; }

        public Guid BudgetTitleId { get; set; }

        public string BudgetTitleName { get; set; }

        public Guid SpecialityId { get; set; }

        public string SpecialityDescription { get; set; }

        public Guid WorkFrontId { get; set; }

        public string WorkFrontCode { get; set; }

        public Guid? ProjectPhaseId { get; set; }

        public string ProjectPhaseCode { get; set; }

        public string ProjectPhaseDescription { get; set; }

        //public DateTime BlueprintDate { get; set; }

        //public string BlueprintDateStr => BlueprintDate.ToDateString();

        public string Description { get; set; }

        public string Sheet { get; set; }

        public Guid? BlueprintTypeId { get; set; }

        public string BlueprintTypeDescription { get; set; }
    } 
}
